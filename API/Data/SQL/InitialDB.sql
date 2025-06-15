SELECT TOP (1000) [Id]
      ,[UserName]
      ,[CreatedAt]
      ,[UpdatedAt]
  FROM [MeetYourMatch].[dbo].[AppUsers]

ALTER TABLE AppUsers
ADD CONSTRAINT DF_AppUsers_CreatedAt DEFAULT GETUTCDATE() FOR CreatedAt;

ALTER TABLE AppUsers
ADD CONSTRAINT DF_AppUsers_UpdatedAt DEFAULT GETUTCDATE() FOR UpdatedAt;

DBCC CHECKIDENT ('appusers', RESEED, 0);
delete from AppUsers;

