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

        public async Task<ServiceResponse<FightResutlDto>> Fight(FightRequestDto fightRequestDto)
        {
            var response = new ServiceResponse<FightResutlDto>();
            var log = new List<string>();
            try
            {
                var characters = await _context.Characters
                        .Include(c => c.Weapon)
                        .Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                        .Where(c => fightRequestDto.CharacterIds.Contains(c.Id))
                        .ToListAsync();

                if (characters.Count >= 2)
                {
                    var defeated = false;

                    while (!defeated)
                    {
                        foreach (var attacker in characters)
                        {
                            var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                            var opponent = opponents[new Random().Next(opponents.Count)];

                            var damage = 0;
                            var attackUsed = string.Empty;

                            var useWeapon = new Random().Next(2) == 0;

                            if (useWeapon)
                            {
                                attackUsed = attacker.Weapon?.Name;
                                damage = DoWeaponAttack(attacker, opponent);
                            }
                            else
                            {
                                var randomSkill = new Random().Next(attacker.CharacterSkills.Count);
                                attackUsed = attacker.CharacterSkills.FirstOrDefault(cs => cs.SkillId == randomSkill)?.Skill.Name;
                                damage = DoSkillAttack(randomSkill, attacker, opponent);
                            }

                            log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0? damage : 0)} damage.");

                            if (opponent.HitPoints <= 0)
                            {
                                defeated = true;

                                attacker.Victories++;
                                opponent.Defeats++;

                                log.Add($"{opponent.Name} has been defeated!");
                                log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                                break;
                            }
                        }
                    }

                    characters.ForEach(c =>
                    {
                        c.Fights++;
                        c.HitPoints = 100;
                    });

                    await _context.SaveChangesAsync();

                    response.Data = new FightResutlDto { Log = log};
                }
                else
                {
                    response.Message = "insufficient characters";
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
                    int damage = DoSkillAttack(skillAttackDto.SkillId, attacker, opponent);

                    if (opponent.HitPoints <= 0)
                    {
                        response.Message = $"{opponent.Name} has been defeated!";
                        attacker.Fights++;
                        opponent.Fights++;

                        opponent.Defeats++;
                        attacker.Victories++;

                        attacker.HitPoints = 100;
                        opponent.HitPoints = 100;
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

        private int DoSkillAttack(int skillId, Character attacker, Character opponent)
        {
            var damage = attacker.CharacterSkills
                .FirstOrDefault(cs => cs.SkillId == skillId)?.Skill.Damage + new Random().Next(attacker.Intelligence)??0;

            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;

            return damage;
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
                    int damage = DoWeaponAttack(attacker, opponent);

                    if (opponent.HitPoints <= 0)
                    {
                        response.Message = $"{opponent.Name} has been defeated!";
                        attacker.Fights++;
                        opponent.Fights++;

                        opponent.Defeats++;
                        attacker.Victories++;

                        attacker.HitPoints = 100;
                        opponent.HitPoints = 100;
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

        private int DoWeaponAttack(Character attacker, Character opponent)
        {
            var damage = attacker.Weapon?.Damage + new Random().Next(attacker.Strength)??0;
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;

            return damage;
        }
    }
}
