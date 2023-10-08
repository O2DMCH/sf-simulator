﻿using FluentValidation;
using SFSimulator.API.Requests;
using SFSimulator.Core;

namespace SFSimulator.API.Validation.Validators;

public class SimulateDaysRequestValidator : AbstractValidator<SimulateDaysRequest>
{
    public SimulateDaysRequestValidator(IGameLogic gameLogic)
    {
        RuleFor(o => o.Level).InclusiveBetween(1, 800);
        RuleFor(o => o.Experience).GreaterThanOrEqualTo(0).Custom((experience, context) =>
        {
            var level = context.InstanceToValidate.Level;
            if (level <= 0)
            {
                return;
            }
            var maxExperience = gameLogic.GetExperienceForNextLevel(level);
            if (experience > maxExperience - 1)
            {
                context.AddFailure($"Experience for level: {level} can't exceed {maxExperience - 1}");
            }
        });
        RuleFor(o => o.BaseStat).GreaterThan(-1);
        RuleFor(o => o.DaysCount).InclusiveBetween(1, 3000);
        RuleFor(o => o.Class).IsInEnum();
        RuleFor(o => o.Strength).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Dexterity).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Intelligence).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Constitution).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Luck).GreaterThanOrEqualTo(0);
        RuleFor(o => o.Armor).GreaterThanOrEqualTo(0);
        RuleFor(o => o.FirstWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.SecondWeapon).SetValidator(new WeaponValidator());
        RuleFor(o => o.RuneBonuses).NotNull().SetValidator(new ResistanceRuneBonusesValidator());
        RuleFor(o => o.GladiatorLevel).InclusiveBetween(0, 15);
        RuleFor(o => o.SoloPortal).InclusiveBetween(0, 50);
        RuleFor(o => o.GuildPortal).InclusiveBetween(0, 50);
    }
}