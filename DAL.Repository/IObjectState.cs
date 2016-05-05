using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repository
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}
