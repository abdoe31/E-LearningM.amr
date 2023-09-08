using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL;

public interface ILectureManger
{

    int addLecture(AddLectureDTO addlecturedto);
    int AddAcessToUser(List<AddLectureAcessDto> addLectureAcessDtos);

    int DeleteLecture(Deletedto deletedto);
    int UpdateLecture(UpdateLectureDto updateLectureDto);

    List<LectureDetailsDto> GetLectureList(int Classid);

    UserLecturedto GetStudentLectureAttendence(string Studentid);
    LectureAttendanceDTO GetLectureAttendance(int lectureId);
    List<Codegenerateddto> GenerateCodes(PostCodegenerateddto postCodegenerateddto);
    List<GetCodesDTO> GetCodes(int Lectureid);

    UsersCLass  GetLectureWithUsers(int   Userid);

    int AcessLectureByCode (string code , string userid);


    StartendLecture startWatching(int userLectureid);
    List<GetLecturetowatchDto> getLecturetowatch(string userid);

}