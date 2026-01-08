-- ====================================
-- BASE DE DATOS ORIGEN
-- ====================================

CREATE DATABASE BD_Origen;
GO

USE BD_Origen;
GO

-- Tabla de Clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150),
    Telefono NVARCHAR(20),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- Tabla de Productos
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    NombreProducto NVARCHAR(150) NOT NULL,
    Categoria NVARCHAR(50),
    Precio DECIMAL(10,2),
    Stock INT,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Tabla de Pedidos
CREATE TABLE Pedidos (
    PedidoID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT FOREIGN KEY REFERENCES Clientes(ClienteID),
    FechaPedido DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10,2),
    Estado NVARCHAR(20),
    Procesado BIT DEFAULT 0
);

-- Tabla de Detalle de Pedidos
CREATE TABLE DetallePedidos (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT FOREIGN KEY REFERENCES Pedidos(PedidoID),
    ProductoID INT FOREIGN KEY REFERENCES Productos(ProductoID),
    Cantidad INT,
    PrecioUnitario DECIMAL(10,2),
    Subtotal DECIMAL(10,2)
);

-- ====================================
-- DATOS DE EJEMPLO
-- ====================================

-- Insertar Clientes
INSERT INTO Clientes (Nombre, Apellido, Email, Telefono) VALUES
('Juan', 'Pérez', 'juan.perez@email.com', '7777-1111'),
('María', 'González', 'maria.gonzalez@email.com', '7777-2222'),
('Carlos', 'Rodríguez', 'carlos.rodriguez@email.com', '7777-3333'),
('Ana', 'Martínez', 'ana.martinez@email.com', '7777-4444'),
('Luis', 'Hernández', 'luis.hernandez@email.com', '7777-5555');

-- Insertar Productos
INSERT INTO Productos (NombreProducto, Categoria, Precio, Stock) VALUES
('Laptop Dell XPS 15', 'Electrónica', 1299.99, 15),
('Mouse Logitech MX Master', 'Accesorios', 99.99, 50),
('Teclado Mecánico RGB', 'Accesorios', 149.99, 30),
('Monitor Samsung 27"', 'Electrónica', 349.99, 20),
('Audífonos Sony WH-1000XM4', 'Audio', 299.99, 25),
('Webcam Logitech C920', 'Accesorios', 79.99, 40),
('Disco SSD 1TB', 'Almacenamiento', 129.99, 60),
('RAM 16GB DDR4', 'Componentes', 89.99, 45);

-- Insertar Pedidos
INSERT INTO Pedidos (ClienteID, FechaPedido, Total, Estado, Procesado) VALUES
(1, DATEADD(day, -5, GETDATE()), 1399.98, 'Pendiente', 0),
(2, DATEADD(day, -4, GETDATE()), 449.98, 'Pendiente', 0),
(3, DATEADD(day, -3, GETDATE()), 1679.97, 'Pendiente', 0),
(4, DATEADD(day, -2, GETDATE()), 299.99, 'Pendiente', 0),
(5, DATEADD(day, -1, GETDATE()), 559.97, 'Pendiente', 0),
(1, GETDATE(), 219.98, 'Pendiente', 0),
(3, GETDATE(), 729.98, 'Pendiente', 0);

-- Insertar Detalle de Pedidos
INSERT INTO DetallePedidos (PedidoID, ProductoID, Cantidad, PrecioUnitario, Subtotal) VALUES
-- Pedido 1
(1, 1, 1, 1299.99, 1299.99),
(1, 2, 1, 99.99, 99.99),
-- Pedido 2
(2, 4, 1, 349.99, 349.99),
(2, 2, 1, 99.99, 99.99),
-- Pedido 3
(3, 1, 1, 1299.99, 1299.99),
(3, 5, 1, 299.99, 299.99),
(3, 3, 1, 149.99, 149.99),
-- Pedido 4
(4, 5, 1, 299.99, 299.99),
-- Pedido 5
(5, 3, 2, 149.99, 299.98),
(5, 7, 2, 129.99, 259.98),
-- Pedido 6
(6, 7, 1, 129.99, 129.99),
(6, 8, 1, 89.99, 89.99),
-- Pedido 7
(7, 1, 1, 1299.99, 1299.99),
(7, 6, 1, 79.99, 79.99);

GO