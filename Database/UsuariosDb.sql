USE VeterinariaDb;
GO
IF OBJECT_ID('dbo.Usuarios') IS NOT NULL DROP TABLE dbo.Usuarios;
GO
CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Correo NVARCHAR(150) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(30) NOT NULL
);
CREATE UNIQUE INDEX UX_Usuarios_Correo ON dbo.Usuarios(Correo);
GO
INSERT INTO dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
 ('recepcion@veterinaria.com', '$2b$11$/sxoIUtuhY/jPsjol/XxeO9WnjCboUgq4Rem/RsnTzY/nI51kDZ2G', 'Recepcionista'),
 ('vet@veterinaria.com',       '$2b$11$mrTBzCAFN.qAQVWaCOuS3OnchFIk2lRZuGNot7hlx9PYnaF6Hzx9y', 'Veterinario');
GO
