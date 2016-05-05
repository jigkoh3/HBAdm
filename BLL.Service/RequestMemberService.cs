using DAL.Entity.Models;
using DAL.Repository;
using MailUtility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class RequestMemberService : IRequestMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestMemberService([Dependency("UnitOfWork")]IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Register(RequestMember value)
        {
            try
            {
                value.Created = DateTime.Now;
                value.RegisterStatus = "Wait";

                _unitOfWork.Repository<RequestMember>().Insert(value);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetMemberStatus(RequestMember value, string password)
        {
            try
            {
                var member = _unitOfWork.Repository<RequestMember>()
                            .Query()
                            .Filter(f => f.Email.ToUpper() == value.Email.ToUpper())
                            .Get()
                            .FirstOrDefault();

                if (member != null)
                {
                    member.HN = value.HN;
                    member.Updated = DateTime.Now;
                    member.UpdatedBy = value.UpdatedBy;

                    if (value.RegisterStatus.ToUpper() == "APPROVED")
                        member.RegisterStatus = "Approved";
                    else
                        member.RegisterStatus = "Rejected";

                    _unitOfWork.Repository<RequestMember>().Update(member);
                    _unitOfWork.Save();

                    SendMailResult(member, password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMailResult(RequestMember member, string password)
        {
            try
            {
                MailModel model = new MailModel
                {
                    Body = MailBody(member, password),
                    Cc = "tum_cs@hotmail.com",
                    Credentials = true,
                    EnableSsl = true,
                    From = "mdarath@gmail.com",
                    Host = "smtp.gmail.com",
                    IsBodyHtml = false,
                    Password = "hbreaker46",
                    Port = 587,
                    Subject = "แจ้งผลการอนุมัติเข้าใช้งานระบบ Healthbook",
                    To = member.Email,
                    UseDefaultCredential = false,
                    Username = "mdarath@gmail.com"
                };

                MailProvider.SendMail(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string MailBody(RequestMember member, string password)
        {
            System.Text.StringBuilder stbMailBody = new System.Text.StringBuilder();
            stbMailBody.AppendLine(string.Format("เรียนคุณ {0} {1},{2}{2}", member.FirstName, member.LastName, Environment.NewLine));
            if (member.RegisterStatus.ToUpper() == "APPROVED")
            {
                stbMailBody.AppendLine(string.Format("ขอแสดงความยินดี คุณได้รับการอนุมัติเข้าใช้งานระบบ Healthbook ค่ะ{0}", Environment.NewLine));
                stbMailBody.AppendLine(string.Format("HN : {0}", member.HN));
                stbMailBody.AppendLine(string.Format("Username : {0}", member.Email));
                stbMailBody.AppendLine(string.Format("Password : {0}{1}{1}", password, Environment.NewLine));
            }
            else
            {
                stbMailBody.AppendLine(string.Format("ขอแสดงความเสียใจ คุณไม่ได้รับการอนุมัติเข้าใช้งานระบบ Healthbook ค่ะ{0}{0}", Environment.NewLine));
            }

            stbMailBody.AppendLine(string.Format("ขอแสดงความนับถือ,"));
            stbMailBody.AppendLine(string.Format("Healthbook Team"));

            return stbMailBody.ToString();
        }

        public List<RequestMember> Get()
        {
            try
            {
                var results = _unitOfWork.Repository<RequestMember>()
                            .Query()
                            .OrderBy(o => o.OrderByDescending(x => x.Created))
                            .Get()
                            .ToList();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Hospital> GetHospital()
        {
            return _unitOfWork.Repository<Hospital>().Query().Filter(f => f.RecordStatus == "1").Get().ToList();
        }

        public string Get(string email)
        {
            var result = _unitOfWork.Repository<RequestMember>()
                        .Query()
                        .Filter(f => f.Email.ToUpper() == email.ToUpper())
                        .Get()
                        .FirstOrDefault();

            if (result != null)
                return result.HN;
            else
                return string.Empty;
        }

        public List<RequestMember> GetMember()
        {
            return _unitOfWork.Repository<RequestMember>()
                    .Query()
                    .Filter(f => f.RegisterStatus.ToUpper() != "WAIT")
                    .Get()
                    .ToList();
        }


        public void Delete(string email)
        {
            try
            {
                var result = _unitOfWork.Repository<RequestMember>().Query().Filter(f => f.Email.ToUpper() == email.ToUpper()).Get().FirstOrDefault();
                if (result != null)
                {
                    _unitOfWork.Repository<RequestMember>().Delete(result);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
