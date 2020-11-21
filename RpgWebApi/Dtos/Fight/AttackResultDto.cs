using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgWebApi.Dtos.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; }
        public string Opponent { get; set; }
        public int AttackerHP { get; set; }
        public int OpponentHP { get; set; }
    }
}
