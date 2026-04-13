using Cw5.Data;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var rooms = FakeDataStore.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);

        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            rooms = rooms.Where(r => r.IsActive);

        return Ok(rooms.ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetRoom(int id)
    {
        var room = FakeDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound();

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuilding(string buildingCode)
    {
        var rooms = FakeDataStore.Rooms.Where(r => string.Equals(r.BuildingCode, buildingCode, StringComparison.OrdinalIgnoreCase)).ToList();
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult AddRoom([FromBody] Room room)
    {
        room.Id = FakeDataStore.Rooms.Count != 0 ? FakeDataStore.Rooms.Max(r => r.Id) + 1 : 1;
        FakeDataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var room = FakeDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound();

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = FakeDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
            return NotFound();

        bool hasFutureReservations = FakeDataStore.Reservations.Any(r => r.RoomId == id && r.Date >= DateOnly.FromDateTime(DateTime.Now));
        if (hasFutureReservations)
            return Conflict("Cannot delete room with future reservations.");

        FakeDataStore.Rooms.Remove(room);
        return NoContent();
    }
}