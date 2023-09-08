using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.BL;

public class LectureManger : ILectureManger
{
    private readonly IUnitOfWork _UnitOfWork;
    private readonly ELearningContext _eLearningContext;

    public LectureManger(IUnitOfWork unitOfWork, ELearningContext _eLearningContext)
    {
        _UnitOfWork = unitOfWork;
        this._eLearningContext = _eLearningContext;

    }

    public int AddAcessToUser(List<AddLectureAcessDto> addLectureAcessDtos)
    {
        if (addLectureAcessDtos.IsNullOrEmpty())
        {
            return -1;
        }

        List<UserLecture> UserLectures = new List<UserLecture>();

        foreach (var item in addLectureAcessDtos)
        {

            UserLecture userLecture = new UserLecture
            { Lectureid = item.Lectureid, AcessType = item.AcessType,
                StudentId = item.UserId,
                QuizRequired = item.quizrequird, Duration = item.Duration };

            UserLectures.Add(userLecture);
        }


        _UnitOfWork._UserLecturerepository.AddALL(UserLectures);
        return _UnitOfWork.SaveChanges();
    }

    public int addLecture(AddLectureDTO addlecturedto)

    {


        Lecture lecture = new Lecture
        {
            Header = addlecturedto.Header,
            Assighnmentid = addlecturedto.Assighnmentid,
            Quizid = addlecturedto.Quizid
        ,
            Classid = addlecturedto.Classid,
            number = addlecturedto.number, VideoParts = addlecturedto.addvideos.Select(x => new VideoPart { number = x.number, Url = x.link, PartHeader = x.PartHeader }).ToList()
        };
        _UnitOfWork.lecturerepository.Add(lecture);
        return _UnitOfWork.SaveChanges();
    }



    public int DeleteLecture(Deletedto deletedto)
    {

        var lecture = _eLearningContext.Lectures.Where(x => x.Id == deletedto.id).FirstOrDefault();
        if (lecture == null || lecture.Header != deletedto.name)
        {

            return -1;
        }


        _UnitOfWork.lecturerepository.Delete(lecture);
        return _UnitOfWork.SaveChanges();
    }

    public List<Codegenerateddto> GenerateCodes(PostCodegenerateddto postCodegenerateddto)
    {
        int l = 6;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        var codes = new List<LectureCode>();

        for (int i = 0; i < postCodegenerateddto.NumberofCode; i++)
       
        {

            var code = new string(Enumerable.Repeat(chars, l).Select(s => s[random.Next(s.Length)]).ToArray());
            var one = new LectureCode();

            one.Lectureid = postCodegenerateddto.Lectureid;
            one.Code = code;
            one.duration = postCodegenerateddto.duration;
            one.QuizRequired = postCodegenerateddto.QuizRequird;
            codes.Add(one);
        }
        var lect = _eLearningContext.Lectures.Where(x => x.Id == postCodegenerateddto.Lectureid).FirstOrDefault();


        _eLearningContext.LectureCodes.AddRange(codes);
        _UnitOfWork.SaveChanges();

        return codes.Select(x => new Codegenerateddto { LectureName = lect.Header, Code = x.Code }).ToList();

    }

    public List<GetCodesDTO> GetCodes(int Lectureid)


    {

        var codes = _eLearningContext.LectureCodes.Where(x => x.Lectureid == Lectureid).Include(x=>x.Student);

        if (codes.IsNullOrEmpty())
        {
            return null;
        }
        return codes.Select(x => new GetCodesDTO { Code = x.Code, CodeId = x.Id, Used = x.Used, Usedate = x.Usedate,

             UserName = x.Student != null ?  $"{x.Student.FirstName}  {x.Student.SecondName}  {x.Student.LastName}" :null
        }).ToList();



    }


    public LectureAttendanceDTO GetLectureAttendance(int lectureId)
    {


        var lecture = _eLearningContext.UserLectures.Where(x => x.Lectureid == lectureId).Include(x=>x.Lecture).Include(x=>x.Student);
        if (lecture == null)
        {

            return null;
        }


        return new LectureAttendanceDTO
        {
            LectureName = lecture.FirstOrDefault().Lecture.Header,
            userLectureAttendances = lecture.Select(x =>
        new UserLectureAttendance
        {
            UserName = $"{x.Student.FirstName}  {x.Student.SecondName}  {x.Student.LastName}",
            start = (DateTime)x.Start,
            end = (DateTime)x.End

        }

        ).ToList()
        };

    }

    public List<LectureDetailsDto> GetLectureList(int Classid)
    {
        var Lectures = _eLearningContext.Lectures.Where(x => x.Classid == Classid);


        if (Lectures.IsNullOrEmpty())
        {


            return null;
        }


        return Lectures.Select(x => new LectureDetailsDto { Header = x.Header, Assighnmentid = x.Assighnmentid, AssighnmentName = x.Assighnment.Header,
            ClassName = x.Class.Name, LectureId = x.Id, Quizid = x.Quizid, QuizName = x.Quiz.Header }).ToList();
    }

    public UsersCLass GetLectureWithUsers(int Lectureid)
    {

        var lecture = _eLearningContext.Lectures.Where(x => x.Id == Lectureid).FirstOrDefault();
        if (lecture is null)
        {
            return null;
        }
        var users = _UnitOfWork._Userrepository.GetStudentsByClass((int)lecture.Classid).Users;
        if (users.IsNullOrEmpty())
        {


            return null;
        }
        UsersCLass U = new UsersCLass { LectureId = Lectureid, LectureName = lecture.Header, users = users.Select(x => new Users { id = x.Id,
            userName = $"{x.FirstName}  {x.SecondName}  {x.LastName}", ParentPhone = x.ParentPhoneNumber, Phone = x.StudentPhoneNumber

        }).ToList() };

        return U;

    }

    public UserLecturedto GetStudentLectureAttendence(string Studentid)
    {
        var user = _UnitOfWork._Userrepository.GetUser(Studentid);

        if (user == null)
        {
            return null;
        }

        var userattend = new UserLecturedto { StudentName = $"{user.FirstName}  {user.SecondName}  {user.LastName}",

            lectureuserAcessds = user.UserLectures.Select(x => new lectureuserAcessd { Lectureid = x.Id, AcessType = x.AcessType.ToString(), Start = x.Start

            , End = x.End
             , LectureName = x.Lecture?.Header }).ToList()
        };


        return userattend;
    }

    public int UpdateLecture(UpdateLectureDto updateLectureDto)
    {
        var lecture = _eLearningContext.Lectures.Where(x => x.Id == updateLectureDto.LectureId).FirstOrDefault();

        if (lecture == null) { return -1; }

        lecture.Classid = updateLectureDto.Classid;
        lecture.Header = updateLectureDto.Header;
        lecture.Quizid = updateLectureDto.Quizid;
        lecture.Assighnmentid = updateLectureDto.Assighnmentid;
        return _UnitOfWork.SaveChanges();

    }

    public int AcessLectureByCode(string code, string userid)
    {


        var lecturecode = _eLearningContext.LectureCodes.Where(x => x.Code == code  && x.Used==false).Include(x => x.Lecture).FirstOrDefault();
        if (lecturecode== null)
        {

            return -1;
        }
        lecturecode.StudentId = userid;

        lecturecode.Used = true;
        lecturecode.Usedate = DateTime.Now;

        _UnitOfWork._Userrepository.GetUser(userid).UserLectures.Add( new UserLecture {  Lectureid=lecturecode.Lectureid , AcessType= AcessType.Code,
            Duration= lecturecode.duration, StudentId=  userid , QuizRequired= (bool)lecturecode.QuizRequired 
        });
        return _UnitOfWork.SaveChanges() ; 
    }

    public StartendLecture startWatching(int userLectureid)
    {
        var userlecture = _eLearningContext.UserLectures.Where(x=>x.Id==userLectureid).FirstOrDefault();

        userlecture.Start = DateTime.Now;

        if(userlecture.Duration != null)
        {
            userlecture.End = DateTime.Now.AddDays((int)userlecture.Duration);


        }

       var state =  _UnitOfWork.SaveChanges();
        if (state > 0)
        {


            return new StartendLecture { start = userlecture.Start, end = userlecture.End };
        }
        return null;


    }

    public List<GetLecturetowatchDto> getLecturetowatch (string userid)
    {


      var lectures =    _eLearningContext.UserLectures.Where(x=>x.Start==null || x.End< DateTime.Now  && x.StudentId==userid).Include(x=>x.Lecture).Include(x=> x.Lecture.VideoParts).Include(x=>x.Lecture.Quiz).Include(x=>x.Lecture.Assighnment).OrderBy(x=>x.Lecture.number);

        var lecturestowatch = new List<GetLecturetowatchDto>();


        foreach (var item  in lectures )
        {

            GetLecturetowatchDto getLecturetowatchDto = new GetLecturetowatchDto();

            getLecturetowatchDto.Lectureid = item.Id;
            getLecturetowatchDto.LectureName = item.Lecture.Header;
            //


            if (item.QuizSolved==false  || item.QuizRequired == true)
            {

                getLecturetowatchDto.quizId = item.Lecture.Quizid;
                getLecturetowatchDto.QuizName = item.Lecture?.Quiz?.Header;





            }

            if (item.QuizSolved == false && item.QuizRequired == false    )
            {
                getLecturetowatchDto.quizId = item.Lecture.Quizid;
                getLecturetowatchDto.QuizName = item.Lecture?.Quiz?.Header;

                getLecturetowatchDto.videoPartdto = item.Lecture.VideoParts.OrderBy(x => x.number).Select(x => new videoPartdto { Name = x.PartHeader, Url = x.Url }).ToList();


            }
            if (item.QuizSolved == true )
            {
                getLecturetowatchDto.QuizName = item.Lecture?.Quiz?.Header;

                getLecturetowatchDto.videoPartdto = item.Lecture?.VideoParts.OrderBy(x=>x.number).Select(x => new videoPartdto { Name = x.PartHeader, Url = x.Url }).ToList();


            }
            if (item.AssighmentSolved)
            {
                getLecturetowatchDto.AssighmentName = item.Lecture?.Assighnment?.Header;

            }
            if (!item.AssighmentSolved)
            {
                getLecturetowatchDto.Assighmentid = item.Lecture?.Assighnmentid;
                getLecturetowatchDto.AssighmentName = item.Lecture?.Assighnment?.Header;

            }



            if (item.Start != null)
            {
                getLecturetowatchDto.start = item.Start;
                    getLecturetowatchDto.end = item.End;
                getLecturetowatchDto.startedwatching = true;
            }
            else
            {

                getLecturetowatchDto.start = null;
                getLecturetowatchDto.end = null;
                getLecturetowatchDto.startedwatching = false;


            }



            lecturestowatch.Add(getLecturetowatchDto);
        }


        return lecturestowatch;


    }
}