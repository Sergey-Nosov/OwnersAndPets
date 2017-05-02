USE [master]
GO 

IF db_id('OwnersAndPetsDB') IS NULL
CREATE DATABASE [OwnersAndPetsDB]
GO

USE [OwnersAndPetsDB]
GO
IF OBJECT_ID(N'Pets','U') IS NULL
CREATE TABLE Pets
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(MAX) NOT NULL
)
IF OBJECT_ID(N'Owners','U') IS NULL
CREATE TABLE Owners
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(MAX) NOT NULL
)
IF OBJECT_ID(N'OwnerPets','U') IS NULL
CREATE TABLE OwnerPets
(
	[OwnerId] INT NOT NULL,
    [PetId]   INT NOT NULL,
    CONSTRAINT [PK_OwnerPet] PRIMARY KEY CLUSTERED ([OwnerId] ASC, [PetId] ASC),
    FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Owners] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY ([PetId]) REFERENCES [dbo].[Pets] ([Id])     ON UPDATE CASCADE ON DELETE CASCADE
)
GO

CREATE procedure [dbo].[usp_generateIdentifier]
    @minLen int = 1
    , @maxLen int = 256
    , @seed int output
    , @string varchar(8000) output
as
begin
    set nocount on;
    declare @length int;
    declare @alpha varchar(8000)
        , @digit varchar(8000)
        , @specials varchar(8000)
        , @first varchar(8000)
    declare @step bigint = rand(@seed) * 2147483647;

    select @alpha = 'qwertyuiopasdfghjklzxcvbnm'
        , @digit = '1234567890'
        , @specials = '_@# '
    select @first = @alpha + '_@';

    set  @seed = (rand((@seed+@step)%2147483647)*2147483647);

    select @length = @minLen + rand(@seed) * (@maxLen-@minLen)
        , @seed = (rand((@seed+@step)%2147483647)*2147483647);

    declare @dice int;
    select @dice = rand(@seed) * len(@first),
        @seed = (rand((@seed+@step)%2147483647)*2147483647);
    select @string = substring(@first, @dice, 1);

    while 0 < @length 
    begin
        select @dice = rand(@seed) * 100
            , @seed = (rand((@seed+@step)%2147483647)*2147483647);
        if (@dice < 10) -- 10% special chars
        begin
            select @dice = rand(@seed) * len(@specials)+1
                , @seed = (rand((@seed+@step)%2147483647)*2147483647);
            select @string = @string + substring(@specials, @dice, 1);
        end
        else if (@dice < 10+10) -- 10% digits
        begin
            select @dice = rand(@seed) * len(@digit)+1
                , @seed = (rand((@seed+@step)%2147483647)*2147483647);
            select @string = @string + substring(@digit, @dice, 1);
        end
        else -- rest 80% alpha
        begin
            declare @preseed int = @seed;
            select @dice = rand(@seed) * len(@alpha)+1
                , @seed = (rand((@seed+@step)%2147483647)*2147483647);

            select @string = @string + substring(@alpha, @dice, 1);
        end

        select @length = @length - 1;   
    end
end
GO

CREATE PROCEDURE [dbo].[OwnerGen]
	@rowsCount int = 30
AS
BEGIN
	SET NOCOUNT ON;
DECLARE @name VARCHAR(256)

DECLARE @minNameLength INT
SET @minNameLength = 10

DECLARE @maxNameLength INT
SET @maxNameLength = 15

DECLARE @seed INT
SET @seed = 10

DECLARE @index INT
SET @index = 0
		WHILE @index < @rowsCount
			BEGIN

			exec [dbo].[usp_generateIdentifier] 
				@minLen = @minNameLength,
				@maxLen = @maxNameLength,
				@seed = @seed output,
				@string = @name output

			INSERT INTO [Owners](Name) VALUES(@name)	
			SET @index = @index + 1
			END
END
GO

CREATE PROCEDURE [dbo].[PetGen]
	@rowsCount int = 100
AS
BEGIN
	SET NOCOUNT ON;
DECLARE @name VARCHAR(256)

DECLARE @minNameLength INT
SET @minNameLength = 5

DECLARE @maxNameLength INT
SET @maxNameLength = 15

DECLARE @seed INT
SET @seed = 10

DECLARE @index INT
SET @index = 0
		WHILE @index < @rowsCount
			BEGIN

			exec [dbo].[usp_generateIdentifier] 
				@minLen = @minNameLength,
				@maxLen = @maxNameLength,
				@seed = @seed output,
				@string = @name output

			INSERT INTO Pets (Name) VALUES(@name)	
			SET @index = @index + 1
			END
END

GO
CREATE PROCEDURE OwnerPetsGen
	@rowsCount int = 100
AS
BEGIN
	SET NOCOUNT ON;
	IF OBJECT_ID('tempdb..#TempPetsIds') IS NOT NULL DROP Table #TempPetsIds
	IF OBJECT_ID('tempdb..#TempOwnersIds') IS NOT NULL DROP Table #TempOwnersIds

	SELECT p.Id as 'pVals'
	INTO #TempPetsIds
	FROM Pets as p
	ORDER BY p.Id

	SELECT o.Id as 'oVals'
	INTO #TempOwnersIds
	FROM Owners as o
	ORDER BY o.Id

	DECLARE @countOwnersIds INT = (SELECT COUNT(*) FROM #TempOwnersIds)
	DECLARE @countPetsIds INT = (SELECT COUNT(*) FROM #TempPetsIds)
	DECLARE @index INT = 0
	WHILE @index<@rowsCount
	BEGIN
		DECLARE @OwnerRowNumber INT = CAST(RAND() * @countOwnersIds + 1 as INT) - 1
		DECLARE @PetRowNumber INT = CAST(RAND() * @countPetsIds + 1 as INT) - 1
		DECLARE @OwnerId INT = (SELECT oVals FROM #TempOwnersIds ORDER BY oVals OFFSET @OwnerRowNumber ROWS FETCH NEXT 1 ROWS ONLY)
		DECLARE @PetId INT = (SELECT pVals FROM #TempPetsIds ORDER BY pVals OFFSET @PetRowNumber ROWS FETCH NEXT 1 ROWS ONLY);
		IF NOT EXISTS(SELECT TOP 1 1 FROM OwnerPets WHERE OwnerId = @OwnerId AND PetId = @PetId)
		BEGIN
			INSERT INTO [OwnerPets](OwnerId, PetId)
			VALUES (@OwnerId, @PetId)
			SET @index = @index + 1
		END
	END
	IF OBJECT_ID('tempdb..#TempPetsIds') IS NOT NULL DROP Table #TempPetsIds
	IF OBJECT_ID('tempdb..#TempOwnersIds') IS NOT NULL DROP Table #TempOwnersIds
END

exec PetGen @rowsCount = 100
GO
exec OwnerGen @rowsCount = 35
GO
exec OwnerPetsGen @rowsCount = 110
GO