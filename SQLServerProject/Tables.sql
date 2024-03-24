CREATE TABLE Subscription (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(20) NOT NULL,
    Price DECIMAL(5,2) NOT NULL,
    Features NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Country (
    Id VARCHAR(4) NOT NULL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL
);

CREATE TABLE User_Client (
    Id INT NOT NULL PRIMARY KEY,
    Firebase_UID VARCHAR(30) NOT NULL UNIQUE,
    Name VARCHAR(40),
    Card_Number VARCHAR(16),
    Status INT NOT NULL, -- Se puede usar INT para almacenar valores numéricos
    Email VARCHAR(35) NOT NULL UNIQUE,
    Password VARCHAR(50),
    Subscription_Date DATE NOT NULL,
    Id_Subscription INT NOT NULL,
    Id_Country VARCHAR(4) NOT NULL,
    FOREIGN KEY (Id_Subscription) REFERENCES Subscription (Id),
    FOREIGN KEY (Id_Country) REFERENCES Country (Id)
);

CREATE TABLE Genre (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(20)
);

CREATE TABLE Director (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE Writer (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE Actor (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE Character (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);
--------------
CREATE TABLE Character_Actor (
    Character_Id INT NOT NULL,
    Actor_Id INT NOT NULL,
    FOREIGN KEY (Character_Id) REFERENCES Character (Id),
    FOREIGN KEY (Actor_Id) REFERENCES Actor (Id),
    PRIMARY KEY (Character_Id, Actor_Id)
);

CREATE TABLE Producer (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE Composer (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE Language (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(20)
);

CREATE TABLE Subtitle (
    Id INT NOT NULL PRIMARY KEY,
    Name_Language VARCHAR(20)
);

CREATE TABLE Age_Restriction (
    Id INT NOT NULL PRIMARY KEY,
    Age TINYINT NOT NULL,
    Description NVARCHAR(MAX)
);

CREATE TABLE Music (
    Id INT NOT NULL PRIMARY KEY,
    Title VARCHAR(60) NOT NULL,
    Duration SMALLINT NOT NULL,
    Rate DECIMAL(3,2) NOT NULL
);

CREATE TABLE Music_Composer (
    Music_Id INT NOT NULL,
    Composer_Id INT NOT NULL,
    FOREIGN KEY (Music_Id) REFERENCES Music (Id),
    FOREIGN KEY (Composer_Id) REFERENCES Composer (Id),
    PRIMARY KEY (Music_Id, Composer_Id)
);

CREATE TABLE Multimedia (
    Id INT NOT NULL PRIMARY KEY,
    Title VARCHAR(60) NOT NULL,
    Duration INT NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Premiere_Date DATE NOT NULL,
    Rate DECIMAL(3,2) NOT NULL,
    Sequel_Id INT,
    Prequel_Id INT,
    FOREIGN KEY (Sequel_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Prequel_Id) REFERENCES Multimedia (Id)
);

CREATE TABLE Spin_Off (
    Spin_Off_Id INT NOT NULL,
    Original_Id INT NOT NULL,
    FOREIGN KEY (Spin_Off_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Original_Id) REFERENCES Multimedia (Id),
    PRIMARY KEY (Spin_Off_Id, Original_Id)
);

CREATE TABLE Multimedia_Genre (
    Multimedia_Id INT NOT NULL,
    Genre_Id INT NOT NULL,
    FOREIGN KEY (Multimedia_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Genre_Id) REFERENCES Genre (Id),
    PRIMARY KEY (Multimedia_Id, Genre_Id)
);

CREATE TABLE Multimedia_Actor (
    Multimedia_Id INT NOT NULL,
    Actor_Id INT NOT NULL,
    FOREIGN KEY (Multimedia_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Actor_Id) REFERENCES Actor (Id),
    PRIMARY KEY (Multimedia_Id, Actor_Id)
);

CREATE TABLE Multimedia_Character (
    Multimedia_Id INT NOT NULL,
    Character_Id INT NOT NULL,
    FOREIGN KEY (Multimedia_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Character_Id) REFERENCES Character (Id),
    PRIMARY KEY (Multimedia_Id, Character_Id)
);

CREATE TABLE Multimedia_Producer (
    Multimedia_Id INT NOT NULL,
    Producer_Id INT NOT NULL,
    FOREIGN KEY (Multimedia_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Producer_Id) REFERENCES Producer (Id),
    PRIMARY KEY (Multimedia_Id, Producer_Id)
);

CREATE TABLE Multimedia_Music (
    Multimedia_Id INT NOT NULL,
    Music_Id INT NOT NULL,
    FOREIGN KEY (Multimedia_Id) REFERENCES Multimedia (Id),
    FOREIGN KEY (Music_Id) REFERENCES Music (Id),
    PRIMARY KEY (Multimedia_Id, Music_Id)
);

---------------------------------------------------------------
--                  MOVIES
---------------------------------------------------------------

CREATE TABLE Movie (
    Id INT NOT NULL PRIMARY KEY,
    Age_Restriction_Id INT NOT NULL,
    FOREIGN KEY (Id) REFERENCES Multimedia(Id),
    FOREIGN KEY (Age_Restriction_Id) REFERENCES Age_Restriction(Id)
);

CREATE TABLE Movie_Director (
    Movie_Id INT NOT NULL,
    Director_Id INT NOT NULL,
    FOREIGN KEY (Movie_Id) REFERENCES Movie (Id),
    FOREIGN KEY (Director_Id) REFERENCES Director (Id),
    PRIMARY KEY (Movie_Id, Director_Id)
);

CREATE TABLE Movie_Writer (
    Movie_Id INT NOT NULL,
    Writer_Id INT NOT NULL,
    FOREIGN KEY (Movie_Id) REFERENCES Movie (Id),
    FOREIGN KEY (Writer_Id) REFERENCES Writer (Id),
    PRIMARY KEY (Movie_Id, Writer_Id)
);

CREATE TABLE Movie_Language (
    Movie_Id INT NOT NULL,
    Language_Id INT NOT NULL,
    FOREIGN KEY (Movie_Id) REFERENCES Movie (Id),
    FOREIGN KEY (Language_Id) REFERENCES Language (Id),
    PRIMARY KEY (Movie_Id, Language_Id)
);

CREATE TABLE Movie_Subtitle (
    Movie_Id INT NOT NULL,
    Subtitle_Id INT NOT NULL,
    FOREIGN KEY (Movie_Id) REFERENCES Movie (Id),
    FOREIGN KEY (Subtitle_Id) REFERENCES Subtitle (Id),
    PRIMARY KEY (Movie_Id, Subtitle_Id)
);

CREATE TABLE Movie_History (
    User_Id INT NOT NULL,
    Movie_Id INT NOT NULL,
    Stop_Time TIME NOT NULL,
    FOREIGN KEY (User_Id) REFERENCES User_Client (Id),
    FOREIGN KEY (Movie_Id) REFERENCES Movie (Id),
    PRIMARY KEY (User_Id, Movie_Id)
);

---------------------------------------------------------------
--                  SERIES
---------------------------------------------------------------

CREATE TABLE Serie (
    Id INT NOT NULL PRIMARY KEY,
    Seasons INT NOT NULL,
    FOREIGN KEY (Id) REFERENCES Multimedia(Id)
);

CREATE TABLE Season (
    Id INT NOT NULL PRIMARY KEY,
    Premiere_Date DATE NOT NULL,
    Name VARCHAR(60),
    Description NVARCHAR(MAX),
    Chapters INT NOT NULL,
    Serie_Id INT NOT NULL,
    FOREIGN KEY (Serie_Id) REFERENCES Serie (Id)
);

CREATE TABLE Chapter (
    Id INT NOT NULL PRIMARY KEY,
    Chapter_Number INT NOT NULL,
    Duration INT NOT NULL,
    Name VARCHAR(60),
    Description NVARCHAR(MAX),
    Serie_Id INT NOT NULL,
    Season_Id INT NOT NULL,
    Age_Restriction_Id INT NOT NULL,
    FOREIGN KEY (Age_Restriction_Id) REFERENCES Age_Restriction(Id),
    FOREIGN KEY (Serie_Id) REFERENCES Serie (Id),
    FOREIGN KEY (Season_Id) REFERENCES Season (Id)
);

CREATE TABLE Chapter_Director (
    Chapter_Id INT NOT NULL,
    Director_Id INT NOT NULL,
    FOREIGN KEY (Chapter_Id) REFERENCES Chapter (Id),
    FOREIGN KEY (Director_Id) REFERENCES Director (Id),
    PRIMARY KEY (Chapter_Id, Director_Id)
);

CREATE TABLE Chapter_Writer (
    Chapter_Id INT NOT NULL,
    Writer_Id INT NOT NULL,
    FOREIGN KEY (Chapter_Id) REFERENCES Chapter (Id),
    FOREIGN KEY (Writer_Id) REFERENCES Writer (Id),
    PRIMARY KEY (Chapter_Id, Writer_Id)
);

CREATE TABLE Chapter_Language (
    Chapter_Id INT NOT NULL,
    Language_Id INT NOT NULL,
    FOREIGN KEY (Chapter_Id) REFERENCES Chapter (Id),
    FOREIGN KEY (Language_Id) REFERENCES Language (Id),
    PRIMARY KEY (Chapter_Id, Language_Id)
);

CREATE TABLE Chapter_Subtitle (
    Chapter_Id INT NOT NULL,
    Subtitle_Id INT NOT NULL,
    FOREIGN KEY (Chapter_Id) REFERENCES Chapter (Id),
    FOREIGN KEY (Subtitle_Id) REFERENCES Subtitle (Id),
    PRIMARY KEY (Chapter_Id, Subtitle_Id)
);

CREATE TABLE Chapter_History (
    User_Id INT NOT NULL,
    Chapter_Id INT NOT NULL,
    Stop_Time TIME NOT NULL,
    FOREIGN KEY (User_Id) REFERENCES User_Client (Id),
    FOREIGN KEY (Chapter_Id) REFERENCES Chapter (Id),
    PRIMARY KEY (User_Id, Chapter_Id)
);