using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Online_Resume_Viewer.Models
{
    public class DocumentModel
    {

        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Title { get; set; }
        public string FilePath { get; set; }
        // Add other properties as needed (e.g., DateCreated, FileSize, etc.)
        public byte[] content { get; set; }
    }
}
