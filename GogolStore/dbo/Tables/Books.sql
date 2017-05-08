CREATE TABLE [dbo].[Books] (
    [Id]          INT NOT NULL IDENTITY(1,1),
    [Name]        NVARCHAR (50) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [CategoryId]  INT NULL,
    [AuthorId]    INT NOT NULL,
    [PublisherId] INT NOT NULL,
    [PublishDate] DATETIME      NULL,
    [Price] FLOAT NOT NULL, 
    [Size] NVARCHAR(50) NULL, 
    [ISBN] NVARCHAR(100) NULL, 
    [Illustrator] NVARCHAR(100) NULL, 
    [Weight] NVARCHAR(20) NULL, 
    [Series] NVARCHAR(50) NULL, 
    [Language] NVARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT publish_id FOREIGN KEY (PublisherId)
	REFERENCES Publisher(Id),
	CONSTRAINT author_id FOREIGN KEY (AuthorId)
	REFERENCES Author(Id),
	CONSTRAINT category_id FOREIGN KEY (CategoryId)
	REFERENCES Category(Id)
);

