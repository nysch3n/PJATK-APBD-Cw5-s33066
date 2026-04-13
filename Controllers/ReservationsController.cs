using Cw5.Data;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var reservations = FakeDataStore.Reservations.AsQueryable();

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);

        if (!string.IsNullOrWhiteSpace(status))
            reservations = reservations.Where(r => string.Equals(r.Status, status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations.ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetReservation(int id)
    {
        var reservation = FakeDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult AddReservation([FromBody] Reservation reservation)
    {
        var room = FakeDataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
            return NotFound("Room does not exist.");

        if (!room.IsActive)
            return BadRequest("Room is not active.");

        bool isOverlapping = FakeDataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            r.EndTime > reservation.StartTime);

        if (isOverlapping)
            return Conflict("Reservation time overlaps with an existing reservation.");

        reservation.Id = FakeDataStore.Reservations.Count != 0 ? FakeDataStore.Reservations.Max(r => r.Id) + 1 : 1;
        FakeDataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
    {
        var reservation = FakeDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound();

        bool isOverlapping = FakeDataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            r.StartTime < updatedReservation.EndTime &&
            r.EndTime > updatedReservation.StartTime);

        if (isOverlapping)
            return Conflict("Updated time overlaps with another reservation.");

        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = FakeDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
            return NotFound();

        FakeDataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}