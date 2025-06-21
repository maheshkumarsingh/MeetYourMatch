SELECT TOP (1000) [Id]
      ,[UserName]
      ,[CreatedAt]
      ,[UpdatedAt]
  FROM [MeetYourMatch].[dbo].[AppUsers]

ALTER TABLE AppUsers
ADD CONSTRAINT DF_AppUsers_CreatedAt DEFAULT GETUTCDATE() FOR CreatedAt;

ALTER TABLE AppUsers
ADD CONSTRAINT DF_AppUsers_UpdatedAt DEFAULT GETUTCDATE() FOR UpdatedAt;

-- Insert dummy users
DBCC CHECKIDENT ('appusers', RESEED, 0);
delete from AppUsers;

INSERT INTO [dbo].[AppUsers] (
    UserName, CreatedAt, UpdatedAt, City, Country,
    DateOfBirth, Gender, Interests, Introduction,
    KnownAs, LookingFor
)
VALUES 
('Lynn', GETUTCDATE(), GETUTCDATE(), 'Northchase', 'Guyana', '1991-11-16', 'female',
 'Id mollit mollit minim dolore irure proident tempor excepteur.',
 'Magna qui proident consequat consequat do exercitation aliquip nostrud veniam aliquip duis elit.',
 'Lynn',
 'Magna quis tempor minim non laboris amet dolore occaecat pariatur anim quis.'),
('Alex', GETUTCDATE(), GETUTCDATE(), 'Austin', 'USA', '1988-03-12', 'male',
 'Football, Coding, Hiking.',
 'Passionate backend engineer and occasional mountain climber.',
 'Alex',
 'Looking to collaborate on cool tech and coffee.'),
('Emma', GETUTCDATE(), GETUTCDATE(), 'Berlin', 'Germany', '1993-07-04', 'female',
 'Photography, reading, wine tasting.',
 'Book lover and eternal optimist.',
 'Em',
 'Seeking someone who enjoys art and deep talks.'),
('James', GETUTCDATE(), GETUTCDATE(), 'Toronto', 'Canada', '1990-02-10', 'male',
 'Tennis, music, traveling.',
 'Outgoing guy who loves adventures.',
 'James',
 'A travel partner and Netflix buddy.'),
('Sophia', GETUTCDATE(), GETUTCDATE(), 'Sydney', 'Australia', '1994-12-21', 'female',
 'Beach, yoga, and startup ideas.',
 'Entrepreneurial spirit with a passion for wellness.',
 'Sophia',
 'Let’s build something beautiful together.'),
-- Add 15 more similar entries
('Liam', GETUTCDATE(), GETUTCDATE(), 'Dublin', 'Ireland', '1992-06-18', 'male', 'Rugby, beers, gaming.', 'Love to chill with good music.', 'Liam', 'Looking for fun vibes.'),
('Olivia', GETUTCDATE(), GETUTCDATE(), 'Oslo', 'Norway', '1991-09-22', 'female', 'Skiing, reading.', 'Quiet and creative soul.', 'Liv', 'Someone kind and funny.'),
('Noah', GETUTCDATE(), GETUTCDATE(), 'Paris', 'France', '1987-05-10', 'male', 'Cooking, art.', 'French food nerd.', 'Noah', 'Wine and dine partner.'),
('Ava', GETUTCDATE(), GETUTCDATE(), 'Rome', 'Italy', '1995-08-01', 'female', 'Fashion, design.', 'Curator of pretty things.', 'Ava', 'Someone with vision.'),
('Mason', GETUTCDATE(), GETUTCDATE(), 'Chicago', 'USA', '1990-11-05', 'male', 'Basketball, crypto.', 'Money and motivation.', 'Mason', 'Let’s win together.'),
('Isabella', GETUTCDATE(), GETUTCDATE(), 'Barcelona', 'Spain', '1993-04-14', 'female', 'Art, meditation.', 'Living in the moment.', 'Isa', 'Calm and confident man.'),
('Ethan', GETUTCDATE(), GETUTCDATE(), 'Amsterdam', 'Netherlands', '1992-01-31', 'male', 'Biking, tech.', 'Minimalist and explorer.', 'Ethan', 'Someone spontaneous.'),
('Mia', GETUTCDATE(), GETUTCDATE(), 'Lisbon', 'Portugal', '1996-02-19', 'female', 'Coffee, beaches.', 'Sun and smiles.', 'Mia', 'Soulmate.'),
('Logan', GETUTCDATE(), GETUTCDATE(), 'Mumbai', 'India', '1989-10-09', 'male', 'Coding, cricket.', 'Tech head with a sporty side.', 'Logan', 'Smart and sweet.'),
('Amelia', GETUTCDATE(), GETUTCDATE(), 'Bangalore', 'India', '1995-12-28', 'female', 'Classical dance, AI.', 'Techie with rhythm.', 'Amy', 'Someone caring.'),
('Lucas', GETUTCDATE(), GETUTCDATE(), 'Bangkok', 'Thailand', '1990-03-17', 'male', 'Fitness, stock market.', 'Growth-focused.', 'Luke', 'Driven person.'),
('Charlotte', GETUTCDATE(), GETUTCDATE(), 'Cape Town', 'South Africa', '1991-07-25', 'female', 'Travel, animals.', 'Nature lover.', 'Charlie', 'Animal lover.'),
('Elijah', GETUTCDATE(), GETUTCDATE(), 'Seoul', 'South Korea', '1993-10-11', 'male', 'Anime, coding.', 'Bit of a nerd.', 'Eli', 'Quirky partner.'),
('Harper', GETUTCDATE(), GETUTCDATE(), 'Tokyo', 'Japan', '1994-06-15', 'female', 'Culture, languages.', 'Polyglot wanderer.', 'Harper', 'World explorer.')


select * from AppUsers;

DBCC CHECKIDENT ('photos', RESEED, 0);
delete from photos;
-- Insert dummy photos
INSERT INTO [dbo].[Photos] (
    Url, IsMain, PublicId, AppUserId, CreatedAt, UpdatedAt
)
VALUES
('https://randomuser.me/api/portraits/women/30.jpg', 1, NULL, 1, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/10.jpg', 1, NULL, 2, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/31.jpg', 1, NULL, 3, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/11.jpg', 1, NULL, 4, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/32.jpg', 1, NULL, 5, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/12.jpg', 1, NULL, 6, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/33.jpg', 1, NULL, 7, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/13.jpg', 1, NULL, 8, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/34.jpg', 1, NULL, 9, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/14.jpg', 1, NULL, 10, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/35.jpg', 1, NULL, 11, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/15.jpg', 1, NULL, 12, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/36.jpg', 1, NULL, 13, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/16.jpg', 1, NULL, 14, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/37.jpg', 1, NULL, 15, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/17.jpg', 1, NULL, 16, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/38.jpg', 1, NULL, 17, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/men/18.jpg', 1, NULL, 18, GETUTCDATE(), GETUTCDATE()),
('https://randomuser.me/api/portraits/women/39.jpg', 1, NULL, 19, GETUTCDATE(), GETUTCDATE());

select * from Photos;


-- Update all users with same password hash/salt
UPDATE [dbo].[AppUsers]
SET PasswordHash = 0x89B3D5C7A897EF7AFD62D789E0149A6C4A037D0B6BC5BA7473BD8F1B28D4F2F077D0EF5203919E789E3E3C2395B2C7E90BC56C6D627C816EB7F45A978CC116D2,
    PasswordSalt = 0x75A1B3FBC69EF6A23895DFEB2B1B81A254B163D0B18B8D7BE652D21B1B5C20521C30890F07AC2E71FAE1E3835E0B479EB54A9AEE1546A1DDBD50BE5D2294B185;



