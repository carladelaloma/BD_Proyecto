CREATE TABLE Tareas (
    ID INTEGER UNIQUE NOT NULL PRIMARY KEY,
    Estado BOOLEAN,
    Salas_ID INTEGER,
    Rol_ID INTEGER,
    FOREIGN KEY (Salas_ID) REFERENCES Salas(ID),
    FOREIGN KEY (Rol_ID) REFERENCES Roles(ID)
);

CREATE TABLE Personajes (
    ID INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT,
    Sombrero BOOLEAN,
    Traje BOOLEAN,
    Rol_ID INTEGER,
    FOREIGN KEY (Rol_ID) REFERENCES Roles(ID)
);

CREATE TABLE Muertes (
ID INTEGER UNIQUE NOT NULL PRIMARY KEY AUTOINCREMENT,
Victima INTEGER,
Asesino INTEGER,
Salas_ID INTEGER,
FOREIGN KEY (Victima) REFERENCES  Personajes(ID),
FOREIGN KEY (Asesino) REFERENCES  Personajes(ID),
FOREIGN KEY (Salas_ID) REFERENCES Salas(ID)
);

CREATE TABLE Salas (
ID INTEGER UNIQUE NOT NULL PRIMARY KEY,
Ubicacion TEXT,
Capacidad INTEGER
);

CREATE TABLE Salas-Salas (
ID_1 INTEGER,
ID_2 INTEGER,
PRIMARY KEY (ID_1, ID_2),
FOREING KEY (ID_1) REFERENCES Salas(ID),
FOREING KEY (ID_2) REFERENCES Salas(ID)
);

CREATE TABLE Roles (
  ID INTEGER UNIQUE NOT NULL PRIMARY KEY,
  Potenciador TEXT
);

INSERT INTO Roles (ID, Potenciador) VALUES(
(1, "Velocidad"),
(2, "Daño"),
(3, "Disfraz"),
(4 "Inteligencia")
);

INSERT INTO Salas (ID, Ubicacion, Capacidad) VALUES (
(1, "este", 5),
(2, "oeste", 3,),
(3, "este", 4),
(4, "oeste", 6),
(5, "oeste", 6)
);

INSERT INTO Tareas (ID, Estado, Salas_ID, Rol_ID) VALUES (
(1, false, 1, 2),
(2, false, 1, 2),
(3, false, 4, 1),
(4, false, 3, 4),
(5, false, 2, 3),
(6, false, 2, 1)
);

INSERT INTO Salas-Salas (ID_1, ID_2) VALUES (
(1, 2),
(2, 1),
(3, 1),
(1, 3),
(3, 4),
(4, 3),
(5, 4),
(4, 5),
(2, 5),
(5, 2)
);