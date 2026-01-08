-- ====================================
-- BASE DE DATOS DESTINO
-- ====================================

CREATE DATABASE BD_Destino;
GO

USE BD_Destino;
GO

-- Tabla para almacenar los JSON procesados
CREATE TABLE PedidosJSON (
    RegistroID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT NOT NULL,
    JSONData NVARCHAR(MAX) NOT NULL,
    FechaProcesamiento DATETIME DEFAULT GETDATE(),
    LoteID INT,
    Estado NVARCHAR(20)
);

-- Tabla de Log de Procesamiento
CREATE TABLE LogProcesamiento (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    LoteID INT,
    FechaInicio DATETIME,
    FechaFin DATETIME,
    RegistrosProcesados INT,
    RegistrosError INT,
    Estado NVARCHAR(50),
    Mensaje NVARCHAR(500)
);



-- Índices para mejorar el rendimiento
CREATE INDEX IX_PedidosJSON_PedidoID ON PedidosJSON(PedidoID);
CREATE INDEX IX_PedidosJSON_LoteID ON PedidosJSON(LoteID);
CREATE INDEX IX_LogProcesamiento_FechaInicio ON LogProcesamiento(FechaInicio);
GO