CREATE TABLE [dbo].[Car] (
    [id]            INT             IDENTITY (1, 1) NOT NULL,
    [brand]         NVARCHAR (MAX)  NOT NULL,
    [model]         NVARCHAR (MAX)  NOT NULL,
    [year]          INT             NOT NULL,
    [passagerSeats] INT             NOT NULL,
    [information]   NVARCHAR (MAX)  NOT NULL,
    [price]         DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED ([id] ASC)
);
