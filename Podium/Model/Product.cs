using ProductHuntClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podium.Model
{
    public class Product
    {
        public PHPost Post{ get; set; }
        public int Ranking { get; set; }

        public Product(PHPost post, int ranking)
        {
            Post = post;
            Ranking = ranking;
        }
    }
}
