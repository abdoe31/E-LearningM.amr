using E_Learning.DAL;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public class GetLecturetowatchDto
{
    public int Lectureid { get; set; } 
    public string LectureName { get; set; } = null;

    public int? quizId { get; set; } = null;
    public string? QuizName { get; set; } = null;

    public int? Assighmentid {  get; set; } = null;
    public string? AssighmentName { get; set; } = null;
    public bool? startedwatching { get; set; } = null;
    public DateTime? start { get; set; } = null;
    public DateTime? end { get; set; } = null;
    public List<videoPartdto>? videoPartdto { get; set; } = new List<videoPartdto>(); 
}
public class videoPartdto
{

    public string Name { get; set; }
    public string Url { get; set; }
}