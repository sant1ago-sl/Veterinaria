SET QUOTED_IDENTIFIER ON;
GO
IF DB_ID('VeterinariaDb') IS NULL CREATE DATABASE VeterinariaDb;
GO
USE VeterinariaDb;
GO
IF OBJECT_ID('dbo.Mascotas') IS NOT NULL DROP TABLE dbo.Mascotas;
IF OBJECT_ID('dbo.Duenios') IS NOT NULL DROP TABLE dbo.Duenios;
GO
CREATE TABLE dbo.Duenios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Telefono NVARCHAR(20) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE TABLE dbo.Mascotas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Especie NVARCHAR(50) NOT NULL,
    Raza NVARCHAR(80) NULL,
    Edad INT NOT NULL CHECK (Edad BETWEEN 0 AND 50),
    Peso DECIMAL(6,2) NOT NULL CHECK (Peso > 0),
    Microchip NVARCHAR(30) NULL,
    DuenioId INT NOT NULL,
    CONSTRAINT FK_Mascotas_Duenios FOREIGN KEY (DuenioId) REFERENCES dbo.Duenios(Id)
);
CREATE UNIQUE INDEX UX_Mascotas_Microchip ON dbo.Mascotas(Microchip) WHERE Microchip IS NOT NULL;
GO
INSERT INTO dbo.Duenios (Nombre, Email, Telefono) VALUES
 (N'Ana Torres', 'ana.torres@mail.com', '987654321'),
 (N'Luis Ramos', 'luis.ramos@mail.com', '912345678'),
 (N'María Quispe', 'maria.quispe@mail.com', NULL);
INSERT INTO dbo.Mascotas (Nombre, Especie, Raza, Edad, Peso, Microchip, DuenioId) VALUES
 (N'Firulais', N'Perro', N'Labrador', 5, 28.50, 'MC-0001', 1),
 (N'Michi', N'Gato', N'Siamés', 3, 4.20, 'MC-0002', 1),
 (N'Rex', N'Perro', N'Pastor Alemán', 7, 32.00, NULL, 2),
 (N'Piolín', N'Ave', N'Canario', 1, 0.03, NULL, 3);
GO
