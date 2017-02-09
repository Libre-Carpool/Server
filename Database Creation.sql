CREATE TABLE [dbo].[Fails] (
    [ip]      VARCHAR (50) NULL,
    [TheTime] DATETIME     NOT NULL
);

CREATE TABLE [dbo].[Rides] (
    [phone]       VARCHAR (32)  NOT NULL,
    [destination] VARCHAR (50)  NOT NULL,
    [departure]   VARCHAR (50)  NOT NULL,
    [time]        DATETIME      NOT NULL,
    [comment]     NVARCHAR (50) NOT NULL,
    [through]     NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([phone] ASC)
);

CREATE TABLE [dbo].[Users] (
    [phone]    VARCHAR (32) NOT NULL,
    [password] BINARY  (32) NOT NULL,
    [salt]     VARCHAR (32) NOT NULL,
    [canadd]   BIT          DEFAULT ((0)) NOT NULL,
    [candel]   BIT          DEFAULT ((0)) NOT NULL,
    [canmod]   BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([phone] ASC)
);
