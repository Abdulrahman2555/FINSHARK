using System ;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.Models;

namespace api.Mappers

{ 
    public static class CommentMapper 
    {
        public static CommentDto tocommentdto(this Comment commentModel)
        {  
            return new CommentDto
            {
                Id=commentModel.Id,
               Title=commentModel.Title,
               Content=commentModel.Content,
               CreatedOn=commentModel.CreatedOn,
               StockId=commentModel.StockId   
            };
            
        }
    }     
}
