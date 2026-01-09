-- Dog Walking Manager - Sample Data Script
-- Creates 50 sample walk records with realistic data

PRINT 'Inserting sample data for Dog Walking Manager...'

-- Insert default admin user (password hash for 'admin123')
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, PasswordHash, CreatedDate) 
    VALUES ('admin', 'GR7mrJGQez9rgBazmSXGlokm4E0PnGHUDaf1aN1q5uc=', GETDATE())
END

-- Insert sample clients
INSERT INTO Clients (Name, Phone, CreatedDate) VALUES
('Sarah Johnson', '555-0101', GETDATE()),
('Mike Rodriguez', '555-0102', GETDATE()),
('Emily Chen', '555-0103', GETDATE()),
('David Brown', '555-0104', GETDATE()),
('Lisa Wilson', '555-0105', GETDATE()),
('Alex Thompson', '555-0106', GETDATE()),
('Maria Garcia', '555-0107', GETDATE()),
('James Miller', '555-0108', GETDATE()),
('Ashley Davis', '555-0109', GETDATE()),
('Robert Taylor', '555-0110', GETDATE()),
('Jennifer White', '555-0111', GETDATE()),
('Chris Anderson', '555-0112', GETDATE()),
('Amanda Moore', '555-0113', GETDATE()),
('Steven Clark', '555-0114', GETDATE()),
('Nicole Lewis', '555-0115', GETDATE())

-- Get client IDs for foreign key references
DECLARE @ClientIds TABLE (ClientId INT, RowNum INT)
INSERT INTO @ClientIds (ClientId, RowNum)
SELECT Id, ROW_NUMBER() OVER (ORDER BY Id) FROM Clients WHERE Name IN (
    'Sarah Johnson', 'Mike Rodriguez', 'Emily Chen', 'David Brown', 'Lisa Wilson',
    'Alex Thompson', 'Maria Garcia', 'James Miller', 'Ashley Davis', 'Robert Taylor',
    'Jennifer White', 'Chris Anderson', 'Amanda Moore', 'Steven Clark', 'Nicole Lewis'
)

-- Insert sample dogs
INSERT INTO Dogs (ClientId, Name, Breed, Age, CreatedDate) VALUES
((SELECT ClientId FROM @ClientIds WHERE RowNum = 1), 'Buddy', 'Golden Retriever', 3, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 1), 'Luna', 'Border Collie', 2, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 2), 'Max', 'German Shepherd', 5, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 3), 'Bella', 'Labrador', 4, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 3), 'Charlie', 'Poodle', 6, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 4), 'Rocky', 'Bulldog', 3, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 5), 'Daisy', 'Beagle', 2, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 5), 'Jack', 'Jack Russell', 4, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 6), 'Molly', 'Cocker Spaniel', 5, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 7), 'Zeus', 'Great Dane', 3, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 8), 'Coco', 'Chihuahua', 7, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 8), 'Bailey', 'Australian Shepherd', 2, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 9), 'Ruby', 'Boxer', 4, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 10), 'Duke', 'Rottweiler', 6, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 11), 'Penny', 'Shih Tzu', 3, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 11), 'Oscar', 'Husky', 2, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 12), 'Sadie', 'Yorkshire Terrier', 5, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 13), 'Tucker', 'Dalmatian', 3, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 14), 'Zoe', 'Pomeranian', 4, GETDATE()),
((SELECT ClientId FROM @ClientIds WHERE RowNum = 15), 'Leo', 'Mastiff', 5, GETDATE())

-- Get client and dog IDs for walk records
DECLARE @WalkData TABLE (ClientId INT, DogId INT, DogName VARCHAR(50), ClientName VARCHAR(100))
INSERT INTO @WalkData (ClientId, DogId, DogName, ClientName)
SELECT c.Id, d.Id, d.Name, c.Name
FROM Clients c
INNER JOIN Dogs d ON c.Id = d.ClientId
WHERE c.Name IN (
    'Sarah Johnson', 'Mike Rodriguez', 'Emily Chen', 'David Brown', 'Lisa Wilson',
    'Alex Thompson', 'Maria Garcia', 'James Miller', 'Ashley Davis', 'Robert Taylor',
    'Jennifer White', 'Chris Anderson', 'Amanda Moore', 'Steven Clark', 'Nicole Lewis'
)

-- Insert 50 sample walks with realistic dates and times
INSERT INTO Walks (ClientId, DogId, WalkDateTime, DurationMinutes, Notes, CreatedDate)
SELECT ClientId, DogId, WalkDateTime, DurationMinutes, Notes, GETDATE() FROM (
VALUES 
-- Week 1
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Buddy'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Buddy'), '2025-01-01 08:00:00', 30, 'New Year morning walk! Buddy was very energetic.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Luna'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Luna'), '2025-01-01 16:00:00', 45, 'Luna enjoyed the New Year celebration sounds.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Max'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Max'), '2025-01-02 07:30:00', 60, 'Long winter walk. Max loves the cold weather.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Bella'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Bella'), '2025-01-02 09:00:00', 30, 'Quick morning walk before work. Bella was excited.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Charlie'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Charlie'), '2025-01-02 18:00:00', 25, 'Evening stroll. Charlie was calm and well-behaved.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Rocky'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Rocky'), '2025-01-03 10:00:00', 20, 'Short walk due to cold weather. Rocky did well.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Daisy'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Daisy'), '2025-01-03 14:00:00', 35, 'Daisy met her dog friends at the park.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Jack'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Jack'), '2025-01-03 19:00:00', 40, 'Jack had lots of energy despite the cold.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Molly'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Molly'), '2025-01-04 08:30:00', 30, 'Molly enjoyed the winter morning sunshine.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zeus'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zeus'), '2025-01-04 11:00:00', 50, 'Zeus needed a good long walk to burn energy.'),

-- Week 2  
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Coco'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Coco'), '2025-01-05 15:30:00', 15, 'Short walk for little Coco. Perfect size for her.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Bailey'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Bailey'), '2025-01-05 17:00:00', 45, 'Bailey loves exploring new winter trails.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Ruby'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Ruby'), '2025-01-06 07:00:00', 35, 'Early morning walk. Ruby was full of energy.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Duke'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Duke'), '2025-01-06 12:00:00', 40, 'Midday walk. Duke was very obedient as always.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Penny'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Penny'), '2025-01-06 16:30:00', 25, 'Penny enjoyed the cool afternoon weather.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Oscar'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Oscar'), '2025-01-06 18:30:00', 55, 'Oscar needed extra exercise today. Huskies love winter!'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Sadie'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Sadie'), '2025-01-07 09:30:00', 20, 'Quick walk for little Sadie. She did great.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Tucker'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Tucker'), '2025-01-07 13:00:00', 45, 'Tucker was very playful during the walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zoe'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zoe'), '2025-01-07 15:00:00', 30, 'Zoe met her Pomeranian friend at the dog park.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Leo'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Leo'), '2025-01-07 20:00:00', 35, 'Evening walk for big Leo. He was calm and gentle.'),

-- Continue with more recent dates
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Buddy'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Buddy'), DATEADD(hour, -168, GETDATE()), 30, 'Another great morning with Buddy.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Max'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Max'), DATEADD(hour, -156, GETDATE()), 50, 'Max enjoyed his weekend walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Bella'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Bella'), DATEADD(hour, -144, GETDATE()), 40, 'Bella was excited for her Saturday walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Rocky'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Rocky'), DATEADD(hour, -132, GETDATE()), 25, 'Rocky was a bit sluggish today but did well.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Daisy'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Daisy'), DATEADD(hour, -120, GETDATE()), 35, 'Sunday morning walk with energetic Daisy.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Molly'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Molly'), DATEADD(hour, -108, GETDATE()), 30, 'Molly enjoyed the sunny weather today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zeus'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zeus'), DATEADD(hour, -96, GETDATE()), 60, 'Long Sunday walk for gentle giant Zeus.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Coco'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Coco'), DATEADD(hour, -84, GETDATE()), 15, 'Quick evening walk for tiny Coco.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Ruby'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Ruby'), DATEADD(hour, -72, GETDATE()), 40, 'Ruby was very well-behaved today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Duke'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Duke'), DATEADD(hour, -60, GETDATE()), 45, 'Duke explored new areas of the park.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Penny'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Penny'), DATEADD(hour, -48, GETDATE()), 20, 'Short evening walk for sweet Penny.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Sadie'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Sadie'), DATEADD(hour, -36, GETDATE()), 25, 'Sadie was in a great mood today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Tucker'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Tucker'), DATEADD(hour, -24, GETDATE()), 50, 'Tucker had extra energy today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zoe'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zoe'), DATEADD(hour, -12, GETDATE()), 30, 'Zoe was excited for her walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Leo'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Leo'), DATEADD(hour, -6, GETDATE()), 50, 'Leo had a great walk today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Luna'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Luna'), DATEADD(hour, -3, GETDATE()), 45, 'Recent walk with Luna. She loved the border collie training.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Charlie'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Charlie'), DATEADD(hour, -2, GETDATE()), 30, 'Charlie was happy to get back to routine.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Jack'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Jack'), DATEADD(hour, -1, GETDATE()), 40, 'Jack was full of energy this morning.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Bailey'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Bailey'), DATEADD(minute, -30, GETDATE()), 50, 'Bailey explored new trails today.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Oscar'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Oscar'), DATEADD(minute, -15, GETDATE()), 60, 'Oscar needed a long walk after being inside.'),

-- Additional walks for variety
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Buddy'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Buddy'), DATEADD(day, -14, GETDATE()), 35, 'Two weeks ago - morning walk with Buddy.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Rocky'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Rocky'), DATEADD(day, -13, GETDATE()), 25, 'Midday walk for Rocky two weeks ago.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Molly'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Molly'), DATEADD(day, -12, GETDATE()), 35, 'Molly enjoyed her afternoon walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Ruby'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Ruby'), DATEADD(day, -11, GETDATE()), 45, 'Ruby was playful and energetic.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Sadie'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Sadie'), DATEADD(day, -10, GETDATE()), 20, 'Short walk for Sadie in the cool weather.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Tucker'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Tucker'), DATEADD(day, -9, GETDATE()), 40, 'Tucker enjoyed his evening walk.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zoe'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zoe'), DATEADD(day, -8, GETDATE()), 30, 'Zoe was excited for her walk last week.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Leo'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Leo'), DATEADD(day, -7, GETDATE()), 50, 'Leo had a great walk last week.'),
((SELECT TOP 1 ClientId FROM @WalkData WHERE DogName = 'Zeus'), (SELECT TOP 1 DogId FROM @WalkData WHERE DogName = 'Zeus'), DATEADD(day, -6, GETDATE()), 45, 'Recent walk with gentle giant Zeus.')
) AS WalkValues(ClientId, DogId, WalkDateTime, DurationMinutes, Notes)

PRINT 'Sample data insertion completed!'
PRINT '50 walk records have been created with 15 clients and 20 dogs'
PRINT 'Walks span from January 2025 to recent dates for realistic testing'