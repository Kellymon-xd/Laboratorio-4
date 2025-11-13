USE master;
GO

-- ============================================================
-- CREAR BASE DE DATOS
-- ============================================================
CREATE DATABASE Clinica
COLLATE Modern_Spanish_CI_AI;
GO

USE Clinica;
GO

-- ============================================================
-- TABLA ROL
-- ============================================================
CREATE TABLE ROL (
    Id_Rol TINYINT IDENTITY(1,1) PRIMARY KEY,
    Descripcion_Rol VARCHAR(30) NOT NULL UNIQUE
);
INSERT INTO ROL (Descripcion_Rol)
VALUES ('Administrador'), ('Medico'), ('Secretario');
GO

-- ============================================================
-- SECUENCIA GLOBAL PARA Id_Usuario
-- ============================================================
CREATE SEQUENCE dbo.Seq_IdUsuario
    AS INT
    START WITH 1
    INCREMENT BY 1;
GO

-- ============================================================
-- TABLA USUARIOS (solo roles internos)
-- ============================================================
CREATE TABLE USUARIOS (
    Id_Usuario CHAR(8) PRIMARY KEY, -- generado por trigger
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL CONSTRAINT UQ_USUARIOS_Email UNIQUE,
    Cedula VARCHAR(30) NOT NULL CONSTRAINT UQ_USUARIOS_Cedula UNIQUE,
    Telefono VARCHAR(30) NULL,
    Contrasena VARCHAR(128) NOT NULL, -- hash (SHA2_256 HEX)
    Id_Rol TINYINT NOT NULL,
    Fecha_Registro DATETIME NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT GETDATE(),
	PedirContraseña BIT DEFAULT 1,
    CONSTRAINT FK_Usuarios_Rol FOREIGN KEY (Id_Rol) REFERENCES ROL(Id_Rol),
    CONSTRAINT CHK_EmailFormatoBasico CHECK (Email LIKE '_%@_%._%' AND Email NOT LIKE '% %')
);
GO

-- ============================================================
-- TABLA ACTIVIDAD_USUARIOS
-- ============================================================
CREATE TABLE ACTIVIDAD_USUARIOS (
    Id_Usuario CHAR(8) PRIMARY KEY,
    Activo BIT NOT NULL CONSTRAINT DF_Activo DEFAULT 1,
    Bloqueado BIT NOT NULL CONSTRAINT DF_Bloqueado DEFAULT 0,
    Intentos_Fallidos INT NOT NULL CONSTRAINT DF_Intentos DEFAULT 0,
    Fecha_Bloqueo DATETIME NULL,
    Ultima_Actividad DATETIME NULL,
    CONSTRAINT FK_Actividad_Usuarios FOREIGN KEY (Id_Usuario) REFERENCES USUARIOS(Id_Usuario)
);
GO

-- ============================================================
-- TRIGGER: Insert usuario → genera Id_Usuario (sin pacientes)
-- ============================================================
CREATE TRIGGER dbo.TRG_Insert_Usuario_Clinica
ON dbo.USUARIOS
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tmp TABLE (
        Nombre NVARCHAR(100),
        Apellido NVARCHAR(100),
        Email NVARCHAR(255),
        Cedula VARCHAR(30),
        Telefono VARCHAR(30),
        Contrasena VARCHAR(128),
        Id_Rol TINYINT,
        Id_Usuario CHAR(8)
    );

    INSERT INTO @tmp (Nombre, Apellido, Email, Cedula, Telefono, Contrasena, Id_Rol, Id_Usuario)
    SELECT 
        i.Nombre, i.Apellido, i.Email, i.Cedula, i.Telefono, i.Contrasena, i.Id_Rol,
        CASE i.Id_Rol
            WHEN 1 THEN 'A' + RIGHT('0000000' + CAST(NEXT VALUE FOR dbo.Seq_IdUsuario AS VARCHAR(7)),7)
            WHEN 2 THEN 'M' + RIGHT('0000000' + CAST(NEXT VALUE FOR dbo.Seq_IdUsuario AS VARCHAR(7)),7)
            WHEN 3 THEN 'S' + RIGHT('0000000' + CAST(NEXT VALUE FOR dbo.Seq_IdUsuario AS VARCHAR(7)),7)
            ELSE 'U' + RIGHT('0000000' + CAST(NEXT VALUE FOR dbo.Seq_IdUsuario AS VARCHAR(7)),7)
        END
    FROM inserted i;

    INSERT INTO dbo.USUARIOS (Id_Usuario, Nombre, Apellido, Email, Cedula, Telefono, Contrasena, Id_Rol, Fecha_Registro)
    SELECT Id_Usuario, Nombre, Apellido, Email, Cedula, Telefono, Contrasena, Id_Rol, GETDATE()
    FROM @tmp;

    INSERT INTO dbo.ACTIVIDAD_USUARIOS (Id_Usuario, Activo, Bloqueado, Intentos_Fallidos, Ultima_Actividad)
    SELECT Id_Usuario, 1, 0, 0, GETDATE()
    FROM @tmp;
END;
GO

-- ============================================================
-- TABLAS COMPLEMENTARIAS
-- ============================================================
CREATE TABLE ESPECIALIDADES(
    ID_Especialidad INT IDENTITY PRIMARY KEY,
    Nombre_Especialidad VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL
);
GO

CREATE TABLE TIPO_CONTRATO(
    ID_Contrato INT IDENTITY PRIMARY KEY,
    Descripcion VARCHAR(30) NOT NULL UNIQUE
);
INSERT INTO TIPO_CONTRATO(Descripcion) VALUES ('eventual'), ('permanente');
GO

CREATE TABLE ESTADO_SALUD(
    ID_Estado INT IDENTITY PRIMARY KEY,
    Descripcion VARCHAR(20) NOT NULL UNIQUE
);
INSERT INTO ESTADO_SALUD VALUES ('activo'), ('inactivo');
GO

CREATE TABLE ESTADO_CITA(
    ID_Estado_Cita INT IDENTITY PRIMARY KEY,
    Descripcion VARCHAR(30) NOT NULL UNIQUE
);
INSERT INTO ESTADO_CITA VALUES ('agendada'), ('atendida'), ('cancelada');
GO

-- ============================================================
-- TABLA PACIENTES (independiente de USUARIOS)
-- ============================================================
CREATE TABLE PACIENTES(
    ID_Paciente INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Cedula VARCHAR(30) NOT NULL UNIQUE,
    Email NVARCHAR(255) NULL,
    Telefono VARCHAR(30) NULL,
    Fecha_Nacimiento DATE NOT NULL,
    Sexo VARCHAR(10) NULL,
    Direccion VARCHAR(200) NULL,
    ContactoEmergencia VARCHAR(100) NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- ============================================================
-- TABLA MEDICOS (sí referencian USUARIOS)
-- ============================================================
CREATE TABLE MEDICOS(
    ID_Medico INT IDENTITY PRIMARY KEY,
    Id_Usuario CHAR(8) NOT NULL UNIQUE,
    ID_Especialidad INT NOT NULL,
    ID_Contrato INT NOT NULL,
    Horario_Atencion VARCHAR(200) NULL,
    Telefono_Consulta VARCHAR(30) NULL,
	Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_MEDICOS_USUARIOS FOREIGN KEY (Id_Usuario) REFERENCES USUARIOS(Id_Usuario),
    CONSTRAINT FK_MEDICOS_ESPECIALIDAD FOREIGN KEY (ID_Especialidad) REFERENCES ESPECIALIDADES(ID_Especialidad),
    CONSTRAINT FK_MEDICOS_CONTRATO FOREIGN KEY (ID_Contrato) REFERENCES TIPO_CONTRATO(ID_Contrato)
);
GO

-- ============================================================
-- CITAS (referencia pacientes y médicos)
-- ============================================================
CREATE TABLE CITAS(
    ID_Cita INT IDENTITY PRIMARY KEY,
    ID_Paciente INT NOT NULL,
    ID_Medico INT NOT NULL,
    Fecha_Cita DATE NOT NULL,
    Hora_Cita TIME NOT NULL,
    ID_Estado_Cita INT NOT NULL,
    CONSTRAINT FK_CITAS_PACIENTE FOREIGN KEY (ID_Paciente) REFERENCES PACIENTES(ID_Paciente),
    CONSTRAINT FK_CITAS_MEDICO FOREIGN KEY (ID_Medico) REFERENCES MEDICOS(ID_Medico),
    CONSTRAINT FK_CITAS_ESTADO FOREIGN KEY (ID_Estado_Cita) REFERENCES ESTADO_CITA(ID_Estado_Cita)
);
GO

-- ============================================================
-- Atención médica
-- ============================================================
CREATE TABLE ATENCION_MEDICA(
    ID_Atencion INT IDENTITY PRIMARY KEY,
    ID_Cita INT NOT NULL,
    Fecha_Atencion DATETIME NOT NULL DEFAULT GETDATE(),
    Motivo_Consulta VARCHAR(300) NOT NULL,
    Diagnostico VARCHAR(300) NULL,
    Observaciones VARCHAR(400) NULL,
    CONSTRAINT FK_ATENCION_CITA FOREIGN KEY (ID_Cita)
        REFERENCES CITAS(ID_Cita)
);
GO


CREATE TABLE ANTECEDENTES_MEDICOS(
    ID_Antecedente INT IDENTITY PRIMARY KEY,
    ID_Paciente INT NOT NULL UNIQUE,
    Alergias VARCHAR(200) NULL,
    Enfermedades_Cronicas VARCHAR(200) NULL,
    Observaciones_Generales VARCHAR(300) NULL,
    Fecha_Registro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ANTECEDENTES_PACIENTE FOREIGN KEY (ID_Paciente)
        REFERENCES PACIENTES(ID_Paciente)
);
GO

-- ============================================================
-- VISTAS
-- ============================================================
CREATE OR ALTER VIEW vw_UsuariosClinica AS
SELECT u.Id_Usuario, u.Nombre, u.Apellido, u.Email, u.Cedula, r.Descripcion_Rol AS Rol,
       au.Activo, au.Bloqueado, au.Intentos_Fallidos, au.Ultima_Actividad
FROM USUARIOS u
LEFT JOIN ROL r ON u.Id_Rol = r.Id_Rol
LEFT JOIN ACTIVIDAD_USUARIOS au ON u.Id_Usuario = au.Id_Usuario;
GO

CREATE OR ALTER VIEW vw_MedicosClinica AS
SELECT m.ID_Medico, m.Id_Usuario, u.Nombre, u.Apellido, u.Email,
       e.Nombre_Especialidad, t.Descripcion AS Contrato,
       m.Horario_Atencion, m.Telefono_Consulta
FROM MEDICOS m
LEFT JOIN USUARIOS u ON m.Id_Usuario = u.Id_Usuario
LEFT JOIN ESPECIALIDADES e ON m.ID_Especialidad = e.ID_Especialidad
LEFT JOIN TIPO_CONTRATO t ON m.ID_Contrato = t.ID_Contrato;
GO

CREATE OR ALTER VIEW vw_PacientesClinica AS
SELECT p.ID_Paciente, p.Nombre, p.Apellido, p.Cedula, p.Email,
       p.Telefono, p.Fecha_Nacimiento, p.Sexo, p.Direccion,
       p.ContactoEmergencia, es.Descripcion AS EstadoSalud
FROM PACIENTES p
LEFT JOIN ESTADO_SALUD es ON p.ID_Estado = es.ID_Estado;
GO

CREATE OR ALTER VIEW vw_CitasClinica AS
SELECT c.ID_Cita, c.Fecha_Cita, c.Hora_Cita, ec.Descripcion AS EstadoCita,
       p.Nombre + ' ' + p.Apellido AS Paciente,
       m.ID_Medico, um.Nombre + ' ' + um.Apellido AS Medico
FROM CITAS c
INNER JOIN PACIENTES p ON c.ID_Paciente = p.ID_Paciente
INNER JOIN MEDICOS m ON c.ID_Medico = m.ID_Medico
LEFT JOIN USUARIOS um ON m.Id_Usuario = um.Id_Usuario
LEFT JOIN ESTADO_CITA ec ON c.ID_Estado_Cita = ec.ID_Estado_Cita;
GO

-- ============================================================
-- PROCEDIMIENTO LOGIN (solo usuarios del sistema)
-- ============================================================
CREATE PROCEDURE dbo.sp_login_usuario_clinica
    @Email NVARCHAR(255),
    @PasswordHash VARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdUsuario CHAR(8);

    IF NOT EXISTS (SELECT 1 FROM USUARIOS WHERE Email = @Email)
    BEGIN
        RAISERROR('Usuario no existe.',16,1);
        RETURN;
    END

    SELECT @IdUsuario = Id_Usuario FROM USUARIOS WHERE Email = @Email;

    IF EXISTS (SELECT 1 FROM ACTIVIDAD_USUARIOS WHERE Id_Usuario = @IdUsuario AND Bloqueado = 1)
    BEGIN
        RAISERROR('Usuario bloqueado.',16,1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1 FROM USUARIOS U
        INNER JOIN ACTIVIDAD_USUARIOS AU ON U.Id_Usuario = AU.Id_Usuario
        WHERE U.Email = @Email AND U.Contrasena = @PasswordHash AND AU.Activo = 1 AND AU.Bloqueado = 0
    )
    BEGIN
        UPDATE ACTIVIDAD_USUARIOS
        SET Intentos_Fallidos = 0, Ultima_Actividad = GETDATE()
        WHERE Id_Usuario = @IdUsuario;

        SELECT U.Id_Usuario, U.Nombre, U.Apellido, R.Descripcion_Rol
        FROM USUARIOS U
        INNER JOIN ROL R ON U.Id_Rol = R.Id_Rol
        WHERE U.Id_Usuario = @IdUsuario;
        RETURN;
    END

    UPDATE ACTIVIDAD_USUARIOS
    SET Intentos_Fallidos = Intentos_Fallidos + 1,
        Bloqueado = CASE WHEN Intentos_Fallidos + 1 >= 3 THEN 1 ELSE 0 END,
        Fecha_Bloqueo = CASE WHEN Intentos_Fallidos + 1 >= 3 THEN GETDATE() ELSE NULL END
    WHERE Id_Usuario = @IdUsuario;

    IF EXISTS (SELECT 1 FROM ACTIVIDAD_USUARIOS WHERE Id_Usuario = @IdUsuario AND Bloqueado = 1)
    BEGIN
        RAISERROR('Usuario bloqueado por intentos fallidos.',16,1);
        RETURN;
    END

    RAISERROR('Contraseña incorrecta.',16,1);
END;
GO

-- ============================================================
-- PROCEDIMIENTO GESTIÓN DE USUARIOS (solo admin)
-- ============================================================
CREATE PROCEDURE dbo.sp_gestion_usuario_clinica
    @Operacion CHAR(1),
    @Id_Usuario CHAR(8) = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @Apellido NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL,
    @Cedula VARCHAR(30) = NULL,
    @Telefono VARCHAR(30) = NULL,
    @Contrasena VARCHAR(128) = NULL,
    @Id_Rol TINYINT = NULL,
    @Activo BIT = NULL,
    @Bloqueado BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF @Operacion = 'I'
        BEGIN
            -- Si existe un usuario con misma cédula inactivo → reactivar
            IF EXISTS (SELECT 1 FROM USUARIOS WHERE Cedula = @Cedula AND Activo = 0)
            BEGIN
                UPDATE USUARIOS
                SET Activo = 1,
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Email = @Email,
                    Telefono = @Telefono,
                    Contrasena = @Contrasena,
                    Id_Rol = @Id_Rol,
                    Fecha_Registro = GETDATE()
                WHERE Cedula = @Cedula;

                UPDATE ACTIVIDAD_USUARIOS
                SET Activo = 1, Bloqueado = 0, Intentos_Fallidos = 0, Ultima_Actividad = GETDATE()
                WHERE Id_Usuario = (SELECT Id_Usuario FROM USUARIOS WHERE Cedula = @Cedula);

                SELECT 'Usuario reactivado correctamente.' AS Mensaje;
            END
            ELSE
            BEGIN
                INSERT INTO USUARIOS (Nombre, Apellido, Email, Cedula, Telefono, Contrasena, Id_Rol)
                VALUES (@Nombre, @Apellido, @Email, @Cedula, @Telefono, @Contrasena, @Id_Rol);
                SELECT 'Usuario insertado correctamente.' AS Mensaje;
            END
        END

        ELSE IF @Operacion = 'U'
        BEGIN
            UPDATE USUARIOS
            SET Nombre = ISNULL(@Nombre, Nombre),
                Apellido = ISNULL(@Apellido, Apellido),
                Email = ISNULL(@Email, Email),
                Cedula = ISNULL(@Cedula, Cedula),
                Telefono = ISNULL(@Telefono, Telefono),
                Contrasena = ISNULL(@Contrasena, Contrasena),
                Id_Rol = ISNULL(@Id_Rol, Id_Rol)
            WHERE Id_Usuario = @Id_Usuario;

            IF @Activo IS NOT NULL OR @Bloqueado IS NOT NULL
            BEGIN
                UPDATE ACTIVIDAD_USUARIOS
                SET Activo = ISNULL(@Activo, Activo),
                    Bloqueado = ISNULL(@Bloqueado, Bloqueado),
                    Ultima_Actividad = GETDATE()
                WHERE Id_Usuario = @Id_Usuario;
            END
            SELECT 'Usuario actualizado correctamente.' AS Mensaje;
        END

        ELSE IF @Operacion = 'D'
        BEGIN
            -- Eliminación lógica
            UPDATE USUARIOS
            SET Activo = 0
            WHERE Id_Usuario = @Id_Usuario;

            UPDATE ACTIVIDAD_USUARIOS
            SET Activo = 0, Ultima_Actividad = GETDATE()
            WHERE Id_Usuario = @Id_Usuario;

            SELECT 'Usuario desactivado correctamente (eliminación lógica).' AS Mensaje;
        END

        ELSE
            RAISERROR('Operación inválida.',16,1);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO


CREATE PROCEDURE dbo.sp_gestion_medico_clinica
    @Operacion CHAR(1),               -- 'I' insertar, 'U' actualizar, 'D' eliminar lógico
    @ID_Medico INT = NULL,
    @Id_Usuario CHAR(8) = NULL,
    @ID_Especialidad INT = NULL,
    @ID_Contrato INT = NULL,
    @Horario_Atencion VARCHAR(200) = NULL,
    @Telefono_Consulta VARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF @Operacion = 'I'
        BEGIN
            -- Si el médico existe inactivo, reactivarlo
            IF EXISTS (SELECT 1 FROM MEDICOS WHERE Id_Usuario = @Id_Usuario AND Activo = 0)
            BEGIN
                UPDATE MEDICOS
                SET Activo = 1,
                    ID_Especialidad = ISNULL(@ID_Especialidad, ID_Especialidad),
                    ID_Contrato = ISNULL(@ID_Contrato, ID_Contrato),
                    Horario_Atencion = ISNULL(@Horario_Atencion, Horario_Atencion),
                    Telefono_Consulta = ISNULL(@Telefono_Consulta, Telefono_Consulta)
                WHERE Id_Usuario = @Id_Usuario;

                SELECT 'Médico reactivado correctamente.' AS Mensaje;
            END
            ELSE
            BEGIN
                INSERT INTO MEDICOS (Id_Usuario, ID_Especialidad, ID_Contrato, Horario_Atencion, Telefono_Consulta)
                VALUES (@Id_Usuario, @ID_Especialidad, @ID_Contrato, @Horario_Atencion, @Telefono_Consulta);
                SELECT 'Médico registrado correctamente.' AS Mensaje;
            END
        END

        ELSE IF @Operacion = 'U'
        BEGIN
            UPDATE MEDICOS
            SET ID_Especialidad = ISNULL(@ID_Especialidad, ID_Especialidad),
                ID_Contrato = ISNULL(@ID_Contrato, ID_Contrato),
                Horario_Atencion = ISNULL(@Horario_Atencion, Horario_Atencion),
                Telefono_Consulta = ISNULL(@Telefono_Consulta, Telefono_Consulta)
            WHERE ID_Medico = @ID_Medico;

            SELECT 'Médico actualizado correctamente.' AS Mensaje;
        END

        ELSE IF @Operacion = 'D'
        BEGIN
            UPDATE MEDICOS
            SET Activo = 0
            WHERE ID_Medico = @ID_Medico;

            SELECT 'Médico desactivado correctamente (eliminación lógica).' AS Mensaje;
        END

        ELSE
            RAISERROR('Operación inválida.',16,1);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE dbo.sp_gestion_paciente_clinica
    @Operacion CHAR(1),                   -- 'I', 'U', 'D'
    @ID_Paciente INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @Apellido NVARCHAR(100) = NULL,
    @Cedula VARCHAR(30) = NULL,
    @Email NVARCHAR(255) = NULL,
    @Telefono VARCHAR(30) = NULL,
    @Fecha_Nacimiento DATE = NULL,
    @Sexo VARCHAR(10) = NULL,
    @Direccion VARCHAR(200) = NULL,
    @ContactoEmergencia VARCHAR(100) = NULL,
    @ID_Estado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        IF @Operacion = 'I'
        BEGIN
            -- Si ya existe con la misma cédula inactivo, reactivar
            IF EXISTS (SELECT 1 FROM PACIENTES WHERE Cedula = @Cedula AND Activo = 0)
            BEGIN
                UPDATE PACIENTES
                SET Activo = 1,
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Email = @Email,
                    Telefono = @Telefono,
                    Fecha_Nacimiento = @Fecha_Nacimiento,
                    Sexo = @Sexo,
                    Direccion = @Direccion,
                    ContactoEmergencia = @ContactoEmergencia,
                    ID_Estado = ISNULL(@ID_Estado, ID_Estado)
                WHERE Cedula = @Cedula;

                SELECT 'Paciente reactivado correctamente.' AS Mensaje;
            END
            ELSE
            BEGIN
                INSERT INTO PACIENTES (Nombre, Apellido, Cedula, Email, Telefono, Fecha_Nacimiento, Sexo, Direccion, ContactoEmergencia, ID_Estado)
                VALUES (@Nombre, @Apellido, @Cedula, @Email, @Telefono, @Fecha_Nacimiento, @Sexo, @Direccion, @ContactoEmergencia, @ID_Estado);
                SELECT 'Paciente registrado correctamente.' AS Mensaje;
            END
        END

        ELSE IF @Operacion = 'U'
        BEGIN
            UPDATE PACIENTES
            SET Nombre = ISNULL(@Nombre, Nombre),
                Apellido = ISNULL(@Apellido, Apellido),
                Cedula = ISNULL(@Cedula, Cedula),
                Email = ISNULL(@Email, Email),
                Telefono = ISNULL(@Telefono, Telefono),
                Fecha_Nacimiento = ISNULL(@Fecha_Nacimiento, Fecha_Nacimiento),
                Sexo = ISNULL(@Sexo, Sexo),
                Direccion = ISNULL(@Direccion, Direccion),
                ContactoEmergencia = ISNULL(@ContactoEmergencia, ContactoEmergencia),
                ID_Estado = ISNULL(@ID_Estado, ID_Estado)
            WHERE ID_Paciente = @ID_Paciente;

            SELECT 'Paciente actualizado correctamente.' AS Mensaje;
        END

        ELSE IF @Operacion = 'D'
        BEGIN
            UPDATE PACIENTES
            SET Activo = 0
            WHERE ID_Paciente = @ID_Paciente;

            SELECT 'Paciente desactivado correctamente (eliminación lógica).' AS Mensaje;
        END

        ELSE
            RAISERROR('Operación inválida.',16,1);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO
