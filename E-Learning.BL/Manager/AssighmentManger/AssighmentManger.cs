using E_Learning.BL;
using E_Learning.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.BL
{
    public class AssighmentManger : IAssighmentManger
    {
        private readonly IAssigmentrepository _Assighment;

          public AssighmentManger(IAssigmentrepository Assighment)
        {
            _Assighment = Assighment;

        }
        public bool AddAssigment(AssighmentDto assighment)
        {
            Assighment assighment1 = new Assighment();
            assighment1.FilePath = assighment.FilePath; 
            assighment1.ModelAnswerFilePath = assighment.ModelAnswerFilePath;
            assighment1.Header = assighment.Header;
            assighment1.Updatedat = assighment.Updatedat;
            assighment1.UpdatedBy = assighment.UpdatedBy;
            _Assighment.AddAssigment(assighment1);
            _Assighment.SaveChanges();

            return true;
        }

        public bool AddUserAssighment(AddUserAssighmenstDto userAssighment)
        {

            UserAssighment userAssighment1 = new UserAssighment();
            userAssighment1.Studentid = userAssighment.Studentid;
            userAssighment1.Assighmentid = userAssighment.Assighmentid;
            userAssighment1.UserAnswerFilePath = userAssighment.UserAnswerFilePath;

         if(_Assighment.AddUserAssighment(userAssighment1))
            {
                return true;
            }

            return false;
            
        }

        public IEnumerable<AssighmentDto>? GetAllAssighment()
        {
            IEnumerable<Assighment> Assighmentlist= _Assighment.GetAllAssighment();
         

            IEnumerable<AssighmentDto>? x = Assighmentlist?.Select(p => new AssighmentDto
            {

                FilePath = p.FilePath,
                ModelAnswerFilePath = p.ModelAnswerFilePath,
                Header = p.Header,
                Updatedat = p.Updatedat,
                UpdatedBy = p.UpdatedBy,
                UserAssighments = p.UserAssighments?.Select(p => new UserAssighmenstDto
                {
                    Studentid = p.Studentid,
                    UserAnswerFilePath = p.UserAnswerFilePath,
                    Checked = p.Checked,
                    Comment = p.Comment,
                    Solved = p.Solved,
                }).ToList(),

            });
            return x;
        }

        public AssighmentDto? GetAssighmentById(int id)
        {
            Assighment? assighment = _Assighment.GetAssigmentById(id);

            if( assighment == null)
            {
                return null;
            }

            return new AssighmentDto
            {
                FilePath = assighment.FilePath,
                ModelAnswerFilePath = assighment.FilePath,
                Header = assighment.Header,
                Updatedat = assighment.Updatedat,
                UpdatedBy = assighment.UpdatedBy,
                Classid = assighment.Classid,
                UserAssighments = assighment?.UserAssighments?.Select(p => new UserAssighmenstDto
                {
                    Studentid = p.Studentid,
                    UserAnswerFilePath = p.UserAnswerFilePath,
                    Checked = p.Checked,
                    Comment = p.Comment,
                    Solved = p.Solved,
                }).ToList(),

            };


        }

        public IEnumerable<ReadUserAssighment> ReadUserAssighmentsByUserId(string UserId)
        {


            IEnumerable<UserAssighment>? userAssighment =    _Assighment.GetUserAssighmentByUserId(UserId);
            IEnumerable<ReadUserAssighment> changeListMapUser = userAssighment.Select(p => new ReadUserAssighment
            {
                Studentid = p.Studentid,
                UserAnswerFilePath = p.UserAnswerFilePath,
                Assighmentid = p.Assighmentid,
                Checked = p.Checked,
                Solved = p.Solved,
                Comment = p.Comment,
                AssighmentFile = new ShowAssighmentDto
                {
                    FilePath = p.Assighment.FilePath,
                    Header = p.Assighment.Header,
                    ModelAnswerFilePath = p.Assighment.ModelAnswerFilePath,
                }

                


            }) ;
            return changeListMapUser;
        }

        public bool RemoveAssigment(int assigmentId)
        {
             
              if(_Assighment.RemoveAssigment(assigmentId))
            {
                _Assighment.SaveChanges();
                return true;


            }
            return false;

        }

        public bool UpdateAssigment(EditAssighmentDto editAssighment)
        {
            Assighment assighment = _Assighment.GetAssigmentById(editAssighment.Id);
            assighment.Updatedat = editAssighment.Updatedat;
            assighment.Header = editAssighment.Header;
            assighment.Classid = editAssighment.Classid;
            assighment.UpdatedBy = editAssighment.UpdatedBy;
            assighment.FilePath = editAssighment.FilePath;
            assighment.ModelAnswerFilePath = editAssighment.ModelAnswerFilePath;
            _Assighment.SaveChanges();
            return true;    


        }

        public bool UpdateUserAssighment(EditUserAssighment editUserAssighment)
        {
            UserAssighment assUser = _Assighment.GetUserAssighment(editUserAssighment.Assighmentid , editUserAssighment.Studentid);

            if(assUser == null)
            {
                return false;
            }
            assUser.Checked = editUserAssighment.Checked;
            assUser.Comment = editUserAssighment.Comment;
            _Assighment.SaveChanges();
            return true;
        }
    }
}
