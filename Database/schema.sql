CREATE DATABASE InibitServiceDesk;
GO
USE InibitServiceDesk;
GO

CREATE TABLE Kunde (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(150) NOT NULL,
    Ansprechpartner NVARCHAR(100) NULL,
    Email           NVARCHAR(150) NULL,
    Telefon         NVARCHAR(50)  NULL,
    Ort             NVARCHAR(100) NULL,
    Aktiv           BIT NOT NULL DEFAULT 1
);

CREATE TABLE Anlage (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    KundeId        INT NOT NULL REFERENCES Kunde(Id),
    Bezeichnung    NVARCHAR(150) NOT NULL,
    Seriennummer   NVARCHAR(50)  NULL,
    Standort       NVARCHAR(150) NULL,
    Inbetriebnahme DATETIME2 NULL,
    Kritikalitaet  TINYINT NOT NULL DEFAULT 2 CHECK (Kritikalitaet BETWEEN 1 AND 3)
);

CREATE TABLE Ticket (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    AnlageId     INT NOT NULL REFERENCES Anlage(Id),
    Titel        NVARCHAR(150)  NOT NULL,
    Beschreibung NVARCHAR(2000) NULL,
    Prioritaet   TINYINT NOT NULL CHECK (Prioritaet BETWEEN 1 AND 4),
    Status       TINYINT NOT NULL DEFAULT 1 CHECK (Status BETWEEN 1 AND 4),
    ZugewiesenAn NVARCHAR(100) NULL,
    FaelligAm    DATETIME2 NULL,
    ErstelltAm   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ErledigtAm   DATETIME2 NULL
);

CREATE TABLE TicketKommentar (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    TicketId   INT NOT NULL REFERENCES Ticket(Id),
    Autor      NVARCHAR(100)  NOT NULL,
    Text       NVARCHAR(2000) NOT NULL,
    ErstelltAm DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Indizes für Filter, Joins und Sortierung
CREATE INDEX IX_Anlage_KundeId           ON Anlage(KundeId);
CREATE INDEX IX_Ticket_AnlageId          ON Ticket(AnlageId);
CREATE INDEX IX_Ticket_Status            ON Ticket(Status);
CREATE INDEX IX_Ticket_Prio              ON Ticket(Prioritaet);
CREATE INDEX IX_TicketKommentar_TicketId ON TicketKommentar(TicketId);
GO

-- Testdaten (erfundene Firmen)
INSERT INTO Kunde (Name, Ansprechpartner, Email, Telefon, Ort, Aktiv) VALUES
(N'Stahlwerk Muster GmbH', N'Anna Berger',   N'a.berger@example.at', N'+43 1 000 001', N'Linz',       1),
(N'Klinikum Beispiel',     N'Paul Huber',    N'p.huber@example.at',  N'+43 1 000 002', N'St. Pölten', 1),
(N'Entsorgung Nord AG',    N'Lisa Wagner',   N'l.wagner@example.at', N'+43 1 000 003', N'Krems',      1),
(N'Stadtwerke Test',       N'Martin Gruber', N'm.gruber@example.at', N'+43 1 000 004', N'Tulln',      0);

INSERT INTO Anlage (KundeId, Bezeichnung, Seriennummer, Standort, Inbetriebnahme, Kritikalitaet) VALUES
(1, N'Walzstraße 2',       N'WS-2019-002',  N'Halle B',    '2019-04-01', 1),
(1, N'Kühlanlage Ofen 3',  N'KA-2021-017',  N'Halle C',    '2021-09-15', 2),
(2, N'MRT-Steuerung',      N'MED-2020-101', N'Radiologie', '2020-02-10', 1),
(2, N'Sterilisator OP 2',  N'STE-2018-044', N'OP-Trakt',   '2018-06-20', 2),
(3, N'Presscontainer 7',   N'PC-2022-007',  N'Hof Nord',   '2022-03-01', 3),
(4, N'Pumpstation Süd',    N'PS-2017-003',  N'Wasserwerk', '2017-11-11', 2);

DECLARE @jetzt DATETIME2 = SYSUTCDATETIME();

INSERT INTO Ticket (AnlageId, Titel, Beschreibung, Prioritaet, Status, ZugewiesenAn, FaelligAm, ErstelltAm, ErledigtAm) VALUES
(1, N'Hydraulikdruck schwankt',      N'Druck fällt unter Last ab.',           1, 2, N'Technik A', DATEADD(DAY,  1, @jetzt), DATEADD(DAY,  -2, @jetzt), NULL),
(1, N'Sensor Walze 4 meldet Fehler', N'Fehlercode E-104 seit Schichtbeginn.', 2, 1, NULL,        DATEADD(DAY, -3, @jetzt), DATEADD(DAY, -10, @jetzt), NULL),
(2, N'Kühlmittelstand niedrig',      NULL,                                    3, 3, N'Technik B', DATEADD(DAY,  5, @jetzt), DATEADD(DAY,  -4, @jetzt), NULL),
(3, N'Steuerung startet neu',        N'Unerwarteter Neustart während Scan.',  1, 1, NULL,        DATEADD(DAY,  2, @jetzt), DATEADD(DAY,  -1, @jetzt), NULL),
(3, N'Software-Update einspielen',   NULL,                                    3, 4, N'Technik A', DATEADD(DAY, -5, @jetzt), DATEADD(DAY, -20, @jetzt), DATEADD(DAY, -6, @jetzt)),
(4, N'Türdichtung undicht',          N'Kammer verliert Druck.',               2, 2, N'Technik C', DATEADD(DAY, -1, @jetzt), DATEADD(DAY,  -7, @jetzt), NULL),
(4, N'Wartung Quartal',              NULL,                                    4, 4, N'Technik C', DATEADD(DAY,-15, @jetzt), DATEADD(DAY, -30, @jetzt), DATEADD(DAY,-16, @jetzt)),
(5, N'Hydraulikschlauch tauschen',   NULL,                                    2, 1, NULL,        DATEADD(DAY,  7, @jetzt), DATEADD(DAY,  -1, @jetzt), NULL),
(5, N'Display flackert',             NULL,                                    4, 1, NULL,        NULL,                     DATEADD(DAY,  -3, @jetzt), NULL),
(6, N'Pumpe 2 laut',                 N'Lagergeräusche hörbar.',               3, 2, N'Technik B', DATEADD(DAY,  3, @jetzt), DATEADD(DAY,  -6, @jetzt), NULL),
(6, N'Notstromtest dokumentieren',   NULL,                                    4, 3, NULL,        DATEADD(DAY, 14, @jetzt), DATEADD(DAY,  -8, @jetzt), NULL),
(2, N'Temperaturalarm Ofen 3',       NULL,                                    1, 4, N'Technik B', DATEADD(DAY, -2, @jetzt), DATEADD(DAY,  -9, @jetzt), DATEADD(DAY, -3, @jetzt)),
(1, N'Schmierplan anpassen',         NULL,                                    4, 1, NULL,        DATEADD(DAY, 20, @jetzt), DATEADD(DAY,  -2, @jetzt), NULL),
(3, N'Netzwerkverbindung instabil',  N'Verbindung bricht stündlich ab.',      2, 3, N'Technik A', DATEADD(DAY, -4, @jetzt), DATEADD(DAY, -12, @jetzt), NULL);

INSERT INTO TicketKommentar (TicketId, Autor, Text, ErstelltAm) VALUES
(1,  N'Technik A',   N'Druckspeicher geprüft, Ventil verdächtig.', DATEADD(DAY,  -1, @jetzt)),
(1,  N'Anna Berger', N'Bitte um Rückmeldung bis morgen.',          DATEADD(HOUR, -5, @jetzt)),
(6,  N'Technik C',   N'Ersatzdichtung bestellt.',                  DATEADD(DAY,  -3, @jetzt)),
(14, N'Technik A',   N'Warte auf Freigabe der Kunden-IT.',         DATEADD(DAY,  -5, @jetzt));