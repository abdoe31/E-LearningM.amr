using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public class Assigmentrepository : IAssigmentrepository
    {
        private readonly ELearningContext _eLearningContext;
        public Assigmentrepository(ELearningContext eLearningContext)
        {

            _eLearningContext = eLearningContext;
        }
        public bool AddAssigment(Assighment assighment)
        {
            _eLearningContext.Assighments.Add(assighment);  
            return true;
        }

        public IEnumerable<Assighment>? GetAllAssighment()
        {
            IEnumerable<Assighment>? x= _eLearningContext?.Assighments.Include(P => P.UserAssighments).ToList();
            return x;
        }
        
        public Assighment? GetAssigmentById(int id)
        {
            Assighment? x = _eLearningContext?.Assighments?.Include(P => P.UserAssighments).Where(B => B.Id == id).FirstOrDefault();
            return x;
        }

        public bool RemoveAssigment(int assigmentId)
        {
            Assighment? x =  _eLearningContext?.Assighments?.Include(P => P.UserAssighments).Where(B => B.Id == assigmentId)?.FirstOrDefault();
            if (x == null)
            {
                return false;
            }
            _eLearningContext.Assighments.Remove(x);
            return true;
                }

        public bool UpdateAssigment(Assighment editAssighment)
        {
            return true;
        }

        public int SaveChanges()
        {
            return _eLearningContext.SaveChanges();
        }

        public UserAssighment? GetUserAssighment(int? assigmentId, string? UserId)
        {
            UserAssighment? userAssighment = _eLearningContext?.UserAssighments?.Where(P=>P.Assighmentid == assigmentId && P.Studentid == UserId).FirstOrDefault();
           
            return userAssighment;
        }

        public bool AddUserAssighment(UserAssighment userAssighment)
        {
            UserAssighment? x = GetUserAssighment(userAssighment.Assighmentid , userAssighment.Studentid);
            if (x == null || x.Solved == false)
            {
                _eLearningContext?.UserAssighments?.Add(userAssighment);
                userAssighment.Solved = true;
                this.SaveChanges();
                return true;
            }
            return false;

        }

        public IEnumerable<UserAssighment> GetUserAssighmentByUserId(string UserId)
        {
            IEnumerable< UserAssighment>? userAssighment = _eLearningContext?.UserAssighments?.Where(P => P.Studentid == UserId).Include(p => p.Assighment).ToList();
            return userAssighment ?? Enumerable.Empty<UserAssighment>();    
        }
    }
}
