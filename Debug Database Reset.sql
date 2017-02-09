DELETE FROM Users;
INSERT INTO Users (phone, password, salt, canadd, candel, canmod)
VALUES ('0521234567', HASHBYTES('SHA2_256', N'admin123'), '123456', '1', '1', '1');

DELETE FROM Rides
INSERT INTO Rides (phone, destination, departure, time, comment, through)
VALUES('0522234567', 'ChIJ7-RHdRGjAhURL2OGWhptgJc', 'ChIJxRQE9XRQAhURAgSWZPXXP8s', '2016-07-26 07:00:00.000', N'', N'')
INSERT INTO Rides (phone, destination, departure, time, comment, through)
VALUES('0523234567', 'ChIJ7-RHdRGjAhURL2OGWhptgJc', 'ChIJxRQE9XRQAhURAgSWZPXXP8s', '2016-07-26 12:00:00.000', N'', N'')
INSERT INTO Rides (phone, destination, departure, time, comment, through)
VALUES('0524234567', 'ChIJ7-RHdRGjAhURL2OGWhptgJc', 'ChIJxRQE9XRQAhURAgSWZPXXP8s', '2016-07-28 17:00:00.000', N'', N'')
INSERT INTO Rides (phone, destination, departure, time, comment, through)
VALUES('0525234567', 'ChIJ7-RHdRGjAhURL2OGWhptgJc', 'ChIJxRQE9XRQAhURAgSWZPXXP8s', '2016-07-30 07:30:00.000', N'Bring cookies!', N'')

DELETE FROM Fails