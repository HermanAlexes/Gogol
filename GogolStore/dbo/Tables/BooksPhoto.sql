﻿CREATE TABLE [dbo].[BooksPhoto]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [BOOK_ID] INT NULL, 
    [PHOTO] VARBINARY(MAX) NULL
    CONSTRAINT book_id FOREIGN KEY (BOOK_ID)
	REFERENCES Books(Id),
)