using Microsoft.EntityFrameworkCore;
using RpgWebApi.Data;
using RpgWebApi.Dtos.Fight;
using RpgWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgWebApi.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttackDto)
        {
            var response = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                    .FirstOrDefaultAsync(c => c.Id == skillAttackDto.AttackerId && c.CharacterSkills.Any(cs => cs.SkillId == skillAttackDto.SkillId));

                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == skillAttackDto.OpponentId);

                if (attacker != null && opponent != null)
                {
                    var damage = attacker.CharacterSkills
                        .First(cs => cs.SkillId == skillAttackDto.SkillId).Skill.Damage + new Random().Next(attacker.Intelligence);

                    damage -= new Random().Next(opponent.Defense);

                    if (damage > 0)
                        opponent.HitPoints -= damage;

                    if (opponent.HitPoints <= 0)
                    {
                        response.Message = $"{opponent.Name} has been defeated!";
                        attacker.Fights++;
                        opponent.Fights++;

                        opponent.Defeats++;
                        attacker.Victories++;
                    }

                    await _context.SaveChangesAsync();

                    response.Data = new AttackResultDto
                    {
                        Attacker = attacker.Name,
                        Opponent = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints
                    };
                }
                else
                {
                    response.Message = "no such characters";
                    response.Success = false;
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttackDto)
        {
            var response = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _context.Characters.Include(c=>c.Weapon).FirstOrDefaultAsync(c => c.Id == weaponAttackDto.AttackerId);
                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == weaponAttackDto.OpponentId);

                if (attacker != null && opponent != null)
                {
                    var damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
                    damage -= new Random().Next(opponent.Defense);

                    if (damage > 0)
                        opponent.HitPoints -= damage;

                    if (opponent.HitPoints <= 0)
                    { 
                        response.Message = $"{opponent.Name} has been defeated!";
                        attacker.Fights++;
                        opponent.Fights++;

                        opponent.Defeats++;
                        attacker.Victories++;
                    }

                    await _context.SaveChangesAsync();

                    response.Data = new AttackResultDto
                    {
                        Attacker = attacker.Name,
                        Opponent = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints
                    };
                }
                else
                {
                    response.Message = "no such characters";
                    response.Success = false;
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
