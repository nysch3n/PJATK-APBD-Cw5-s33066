using Cw5.Models;

namespace Cw5.Data;

public static class FakeDataStore
{
    public static List<Room> Rooms { get; set; } =
    [
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 30, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Lab 102", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Aula", BuildingCode = "B", Floor = 0, Capacity = 200, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "Sala 204", BuildingCode = "C", Floor = 2, Capacity = 25, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Sala 205", BuildingCode = "C", Floor = 2, Capacity = 10, HasProjector = false, IsActive = true }
    ];

    public static List<Reservation> Reservations { get; set; } =
    [
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Kowalski", Topic = "C# Podstawy", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(10, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 1, OrganizerName = "Anna Nowak", Topic = "ASP.NET", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 30), EndTime = new TimeOnly(12, 0), Status = "confirmed" },
        new Reservation { Id = 3, RoomId = 2, OrganizerName = "Piotr Wiśniewski", Topic = "Konsultacje", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(15, 0), Status = "planned" },
        new Reservation { Id = 4, RoomId = 3, OrganizerName = "Wydział", Topic = "Zebranie", Date = new DateOnly(2026, 5, 15), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(14, 0), Status = "confirmed" }
    ];
}