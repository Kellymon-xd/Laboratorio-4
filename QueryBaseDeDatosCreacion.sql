-- ============================================
-- CREAR BASE DE DATOS
-- ============================================
CREATE DATABASE farmacia
    WITH OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'es_ES.UTF-8'
    LC_CTYPE = 'es_ES.UTF-8'
    TEMPLATE = template0;
\connect farmacia;

-- ============================================
-- TABLA ROLES
-- ============================================
CREATE TABLE rol (
    id_rol SERIAL PRIMARY KEY,
    descripcion_rol VARCHAR(30) NOT NULL UNIQUE
);

INSERT INTO rol (descripcion_rol)
VALUES ('Administrador'), ('Cliente');

-- ============================================
-- SECUENCIAS POR ROL
-- ============================================
CREATE SEQUENCE seq_idusuario_admin START 1;
CREATE SEQUENCE seq_idusuario_cliente START 1;

-- ============================================
-- TABLA USUARIOS
-- ============================================
CREATE TABLE usuarios (
    id_usuario CHAR(8) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    contraseña VARCHAR(64) NOT NULL,
    id_rol INT NOT NULL REFERENCES rol(id_rol),
    CHECK (nombre ~ '^[A-Za-z áéíóúÁÉÍÓÚñÑ]+$'),
    CHECK (apellido ~ '^[A-Za-z áéíóúÁÉÍÓÚñÑ]+$'),
    CHECK (email LIKE '_%@_%._%' AND email NOT LIKE '% %')
);

-- ============================================
-- TABLA MEDICAMENTOS
-- ============================================
CREATE TABLE medicamentos (
    id_medicamento SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    imagen VARCHAR(255),
    cantidad_disponible INT NOT NULL CHECK (cantidad_disponible >= 0),
    precio_unitario NUMERIC(10,2) NOT NULL CHECK (precio_unitario >= 0),
    habilitado BOOLEAN NOT NULL DEFAULT TRUE
);

-- ============================================
-- TABLA PEDIDOS
-- ============================================
CREATE TABLE pedidos (
    id_pedido SERIAL PRIMARY KEY,
    id_cliente CHAR(8) NOT NULL REFERENCES usuarios(id_usuario),
    fecha_pedido TIMESTAMP DEFAULT NOW(),
    total NUMERIC(10,2) NOT NULL CHECK (total >= 0)
);

-- ============================================
-- TABLA DETALLE_PEDIDO
-- ============================================
CREATE TABLE detalle_pedido (
    id_detalle SERIAL PRIMARY KEY,
    id_pedido INT NOT NULL REFERENCES pedidos(id_pedido) ON DELETE CASCADE,
    id_medicamento INT NOT NULL REFERENCES medicamentos(id_medicamento),
    cantidad INT NOT NULL CHECK (cantidad > 0),
    subtotal NUMERIC(10,2) NOT NULL CHECK (subtotal >= 0)
);

-- ============================================
-- INVENTARIO INICIAL
-- ============================================
INSERT INTO medicamentos(nombre, imagen, cantidad_disponible, precio_unitario)
VALUES
('Paracetamol', 'paracetamol.jpg', 100, 0.50),
('Ibuprofeno', 'ibuprofeno.jpg', 50, 1.20),
('Amoxicilina', 'amoxicilina.jpg', 30, 2.50),
('Loratadina', 'loratadina.jpg', 75, 0.75),
('Omeprazol', 'omeprazol.jpg', 200, 0.60);


-- ============================================
-- TRIGGER PARA GENERAR ID_USUARIO SEGÚN ROL
-- ============================================
CREATE FUNCTION generar_id_usuario()
RETURNS TRIGGER AS $$
DECLARE
    nuevo_id CHAR(8);
BEGIN
    IF NEW.id_rol = 1 THEN
        nuevo_id := 'A' || LPAD(NEXTVAL('seq_idusuario_admin')::TEXT, 7, '0');
    ELSIF NEW.id_rol = 2 THEN
        nuevo_id := 'C' || LPAD(NEXTVAL('seq_idusuario_cliente')::TEXT, 7, '0');
    END IF;

    NEW.id_usuario := nuevo_id;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_generar_id_usuario
BEFORE INSERT ON usuarios
FOR EACH ROW
EXECUTE FUNCTION generar_id_usuario();

-- ============================================
-- FUNCIONES / PROCEDIMIENTOS
-- ============================================


-- 🧪 Agregar nuevo medicamento
CREATE FUNCTION agregar_medicamento(
    p_nombre VARCHAR,
    p_imagen VARCHAR,
    p_cantidad INT,
    p_precio NUMERIC
) RETURNS TEXT AS $$
BEGIN
    BEGIN
        INSERT INTO medicamentos(nombre, imagen, cantidad_disponible, precio_unitario)
        VALUES (p_nombre, p_imagen, p_cantidad, p_precio);
        RETURN '✅ Medicamento agregado correctamente.';
    EXCEPTION
        WHEN unique_violation THEN
            RETURN '❌ Error: el medicamento ya existe.';
        WHEN check_violation THEN
            RETURN '❌ Error: algún valor no cumple las restricciones.';
        WHEN others THEN
            RETURN '❌ Error desconocido al agregar medicamento.';
    END;
END;
$$ LANGUAGE plpgsql;

-- 🧱 Modificar medicamento
CREATE FUNCTION modificar_medicamento(
    p_id INT,
    p_nombre VARCHAR DEFAULT NULL,
    p_imagen VARCHAR DEFAULT NULL,
    p_cantidad INT DEFAULT NULL,
    p_precio NUMERIC DEFAULT NULL
) RETURNS TEXT AS $$
BEGIN
    BEGIN
        UPDATE medicamentos
        SET nombre = COALESCE(p_nombre, nombre),
            imagen = COALESCE(p_imagen, imagen),
            cantidad_disponible = COALESCE(p_cantidad, cantidad_disponible),
            precio_unitario = COALESCE(p_precio, precio_unitario)
        WHERE id_medicamento = p_id;

        IF NOT FOUND THEN
            RETURN '❌ Error: medicamento no encontrado.';
        END IF;

        RETURN '✅ Medicamento modificado correctamente.';
    EXCEPTION
        WHEN unique_violation THEN
            RETURN '❌ Error: el nombre ya existe para otro medicamento.';
        WHEN check_violation THEN
            RETURN '❌ Error: algún valor no cumple las restricciones.';
        WHEN others THEN
            RETURN '❌ Error desconocido al modificar medicamento.';
    END;
END;
$$ LANGUAGE plpgsql;


-- 🧱 Eliminar medicamento
CREATE FUNCTION eliminar_medicamento(p_id INT)
RETURNS TEXT AS $$
BEGIN
    BEGIN
        DELETE FROM medicamentos WHERE id_medicamento = p_id;
        IF NOT FOUND THEN
            RETURN '❌ Error: medicamento no encontrado.';
        END IF;
        RETURN '✅ Medicamento eliminado correctamente.';
    EXCEPTION
        WHEN others THEN
            RETURN '❌ Error desconocido al eliminar medicamento.';
    END;
END;
$$ LANGUAGE plpgsql;

-- 🔁 Reabastecer inventario
CREATE FUNCTION reabastecer_medicamento(p_id INT, p_cantidad INT)
RETURNS TEXT AS $$
BEGIN
    BEGIN
        UPDATE medicamentos
        SET cantidad_disponible = cantidad_disponible + p_cantidad
        WHERE id_medicamento = p_id;

        IF NOT FOUND THEN
            RETURN '❌ Error: medicamento no encontrado.';
        END IF;

        RETURN '✅ Inventario reabastecido correctamente.';
    EXCEPTION
        WHEN others THEN
            RETURN '❌ Error desconocido al reabastecer medicamento.';
    END;
END;
$$ LANGUAGE plpgsql;

-- 🧾 Registrar pedido (con descuento automático de stock)
CREATE FUNCTION registrar_pedido(
    p_id_cliente CHAR(8),
    p_medicamentos INT[],
    p_cantidades INT[]
) RETURNS TEXT AS $$
DECLARE
    i INT;
    v_total NUMERIC(10,2) := 0;
    v_subtotal NUMERIC(10,2);
    v_precio NUMERIC(10,2);
    v_pedido_id INT;
BEGIN
    BEGIN
        INSERT INTO pedidos(id_cliente, total) VALUES (p_id_cliente, 0)
        RETURNING id_pedido INTO v_pedido_id;

        FOR i IN 1..array_length(p_medicamentos,1) LOOP
            SELECT precio_unitario INTO v_precio FROM medicamentos
            WHERE id_medicamento = p_medicamentos[i];

            IF NOT FOUND THEN
                RAISE EXCEPTION '❌ Error: medicamento ID % no encontrado.', p_medicamentos[i];
            END IF;

            v_subtotal := v_precio * p_cantidades[i];
            v_total := v_total + v_subtotal;

            INSERT INTO detalle_pedido(id_pedido, id_medicamento, cantidad, subtotal)
            VALUES (v_pedido_id, p_medicamentos[i], p_cantidades[i], v_subtotal);

            UPDATE medicamentos
            SET cantidad_disponible = cantidad_disponible - p_cantidades[i]
            WHERE id_medicamento = p_medicamentos[i];
        END LOOP;

        UPDATE pedidos SET total = v_total WHERE id_pedido = v_pedido_id;

        RETURN '✅ Pedido registrado correctamente.';
    EXCEPTION
        WHEN check_violation THEN
            RETURN '❌ Error: violación de restricción en pedido.';
        WHEN foreign_key_violation THEN
            RETURN '❌ Error: cliente o medicamento no existe.';
        WHEN others THEN
            RETURN '❌ Error desconocido al registrar pedido.';
    END;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- VISTA DE INVENTARIO
-- ============================================

CREATE VIEW vista_inventario AS
SELECT 
    id_medicamento, 
    nombre, 
    imagen, 
    cantidad_disponible, 
    precio_unitario
FROM medicamentos
WHERE habilitado = TRUE;

-- ============================================
-- VISTA DE CLIENTES CON ID
-- ============================================

CREATE VIEW vista_clientes AS
SELECT
    id_usuario,
    nombre || ' ' || apellido AS nombre_completo
FROM usuarios
WHERE id_rol = 2;  -- solo clientes

-- ============================================
-- FUNCION PEDIDOS POR CLIENTE
-- ============================================

CREATE FUNCTION pedidos_por_cliente(p_id_cliente CHAR(8))
RETURNS TABLE(
    id_pedido INT,
    medicamento VARCHAR,
    cantidad INT,
    subtotal NUMERIC(10,2),
    total NUMERIC(10,2),
    fecha_pedido TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        p.id_pedido,
        m.nombre AS medicamento,
        d.cantidad,
        d.subtotal,
        p.total,
        p.fecha_pedido
    FROM pedidos p
    JOIN detalle_pedido d ON d.id_pedido = p.id_pedido
    JOIN medicamentos m ON d.id_medicamento = m.id_medicamento
    WHERE p.id_cliente = p_id_cliente
    ORDER BY p.fecha_pedido DESC;
END;
$$ LANGUAGE plpgsql;


-- ============================================
-- FUNCION DE LOG IN
-- ============================================

CREATE FUNCTION login_usuario(
    p_email VARCHAR,
    p_password VARCHAR
)
RETURNS TABLE(
    id_usuario CHAR(8),
    nombre VARCHAR,
    apellido VARCHAR,
    id_rol INT,
    mensaje TEXT
) AS $$
BEGIN
    -- Verificar si el correo existe
    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE email = p_email) THEN
        RETURN QUERY SELECT NULL::CHAR(8), NULL::VARCHAR, NULL::VARCHAR, NULL::INT, '❌ Error: correo no existe.';
        RETURN;
    END IF;

    -- Verificar contraseña
    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE email = p_email AND contraseña = p_password) THEN
        RETURN QUERY SELECT NULL::CHAR(8), NULL::VARCHAR, NULL::VARCHAR, NULL::INT, '❌ Error: contraseña incorrecta.';
        RETURN;
    END IF;

    -- Si todo es correcto, usa alias
    RETURN QUERY
    SELECT u.id_usuario, u.nombre, u.apellido, u.id_rol, '✅ Login exitoso'
    FROM usuarios u
    WHERE u.email = p_email;
END;
$$ LANGUAGE plpgsql;


-- ============================================
-- FUNCION DE CREAR USUARIO CON VALIDACIONES Y MENSAJES
-- ============================================
CREATE OR REPLACE FUNCTION crear_usuario(
    p_nombre VARCHAR,
    p_apellido VARCHAR,
    p_email VARCHAR,
    p_contraseña VARCHAR,
    p_id_rol INT
)
RETURNS TABLE (
    exito BOOLEAN,
    mensaje TEXT
) AS $$
BEGIN
    -- Validar rol
    IF NOT EXISTS (SELECT 1 FROM rol WHERE id_rol = p_id_rol) THEN
        RETURN QUERY SELECT FALSE, 'Error: el rol no existe.';
        RETURN;
    END IF;

    -- Validaciones previas
    IF p_nombre !~ '^[A-Za-z áéíóúÁÉÍÓÚñÑ]+$' THEN
        RETURN QUERY SELECT FALSE, 'Error: el nombre solo puede contener letras y espacios.';
        RETURN;
    END IF;

    IF p_apellido !~ '^[A-Za-z áéíóúÁÉÍÓÚñÑ]+$' THEN
        RETURN QUERY SELECT FALSE, 'Error: el apellido solo puede contener letras y espacios.';
        RETURN;
    END IF;

    IF p_email NOT LIKE '_%@_%._%' OR p_email LIKE '% %' THEN
        RETURN QUERY SELECT FALSE, 'Error: formato de correo inválido.';
        RETURN;
    END IF;

    IF LENGTH(p_contraseña) < 5 THEN
        RETURN QUERY SELECT FALSE, 'Error: la contraseña debe tener al menos 5 caracteres.';
        RETURN;
    END IF;

    -- Insertar usuario
    BEGIN
        INSERT INTO usuarios(nombre, apellido, email, contraseña, id_rol)
        VALUES (p_nombre, p_apellido, p_email, p_contraseña, p_id_rol);

        RETURN QUERY SELECT TRUE, 'Usuario creado correctamente.';
    EXCEPTION
        WHEN unique_violation THEN
            RETURN QUERY SELECT FALSE, 'Error: el correo ya está registrado.';
        WHEN others THEN
            RETURN QUERY SELECT FALSE, 'Error desconocido al crear el usuario.';
    END;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- VISTA DE PEDIDOS
-- ============================================

CREATE OR REPLACE VIEW vista_pedidos_resumen AS
SELECT 
    p.id_pedido,
    u.nombre || ' ' || u.apellido AS cliente,
    p.total,
    p.fecha_pedido,
p.id_cliente
FROM pedidos p
JOIN usuarios u ON p.id_cliente = u.id_usuario;


-- ============================================
-- FUNCION PARA OBTENER INFO DE PEDIDO
-- ============================================

CREATE FUNCTION obtener_pedido_por_id(p_id_pedido INT)
RETURNS TABLE(
    id_pedido INT,
    cliente VARCHAR,
    fecha TIMESTAMP,
    total NUMERIC(10,2)
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.id_pedido,
        u.nombre || ' ' || u.apellido AS cliente,
        p.fecha_pedido,
        p.total
    FROM pedidos p
    JOIN usuarios u ON p.id_cliente = u.id_usuario
    WHERE p.id_pedido = p_id_pedido;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- FUNCION PARA OBTENER DETALLES DE PEDIDO
-- ============================================

CREATE FUNCTION detalle_pedido_por_id(p_id_pedido INT)
RETURNS TABLE(
    medicamento VARCHAR,
    cantidad INT,
    subtotal NUMERIC(10,2)
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        m.nombre AS medicamento,
        d.cantidad,
        d.subtotal
    FROM detalle_pedido d
    JOIN medicamentos m ON d.id_medicamento = m.id_medicamento
    WHERE d.id_pedido = p_id_pedido
    ORDER BY d.id_detalle;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- FUNCION PARA ELIMINAR MEDICAMENTO
-- ============================================

CREATE OR REPLACE FUNCTION eliminar_medicamento(p_id INT)
RETURNS TEXT AS $$
DECLARE
    tiene_relaciones BOOLEAN;
BEGIN
    -- Verificar si el medicamento está referenciado en otras tablas
    SELECT EXISTS (
        SELECT 1 FROM detalle_pedido WHERE id_medicamento = p_id
        UNION
        SELECT 1 FROM detalle_pedido WHERE id_medicamento = p_id
    ) INTO tiene_relaciones;

    IF tiene_relaciones THEN
        -- Solo deshabilitar si tiene relaciones
        UPDATE medicamentos
        SET habilitado = FALSE
        WHERE id_medicamento = p_id;

        IF NOT FOUND THEN
            RETURN '❌ Error: medicamento no encontrado.';
        END IF;

        RETURN '⚠️ Medicamento deshabilitado (tiene registros asociados).';
    ELSE
        -- Eliminar definitivamente si no hay relaciones
        DELETE FROM medicamentos
        WHERE id_medicamento = p_id;

        IF NOT FOUND THEN
            RETURN '❌ Error: medicamento no encontrado.';
        END IF;

        RETURN '✅ Medicamento eliminado definitivamente.';
    END IF;

EXCEPTION
    WHEN others THEN
        RETURN '❌ Error desconocido al eliminar medicamento.';
END;
$$ LANGUAGE plpgsql;

CREATE EXTENSION IF NOT EXISTS pgcrypto;


-- ADMIN CREADO

SELECT * 
FROM crear_usuario(
    'Admin',
    'Principal',
    'admin@farmacia.com',
    encode(digest('admin123'::bytea, 'sha256'), 'hex'),
    1
);

SELECT * 
FROM crear_usuario(
    'Cliente',
    'Común',
    'cliente@gmail.com',
    encode(digest('user123'::bytea, 'sha256'), 'hex'),
    2
);