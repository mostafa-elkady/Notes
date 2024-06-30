using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Data.Entities;
using Notes.Infrastructure.Data;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        //EndPoints

        //Get All Notes
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            return Ok(await _context.Notes.ToListAsync());
        }
        //Get Specific Note
        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) { return NotFound(); }
            return Ok(note);
        }

        // Add Note
        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
        }

        //Update Note
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note updatedNote)
        {
            //Get the record from database
            var existingNote = await _context.Notes.FindAsync(id);
            // Ensure that this record is exist
            if (existingNote == null) { return NotFound(); }
            // Update The Values
            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.IsVisible = updatedNote.IsVisible;
            // Save The Changes
            await _context.SaveChangesAsync();
            return Ok(existingNote);
        }

        //Delete Note
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            //Get the record from database
            var existingNote = await _context.Notes.FindAsync(id);
            // Ensure that this record is exist
            if (existingNote == null) { return NotFound(); }
            // Delete The Record
            _context.Notes.Remove(existingNote);
            // Save The Changes
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
