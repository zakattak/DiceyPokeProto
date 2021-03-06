using System.Reflection;
using System.Linq;
using System;

public class Move {
    public static Move Tackle = new Move(
        name:"Tackle", desc:"1 DMG\nOverload: +1 DMG", type:MoveType.NORMAL,
        damage:1, cost:1,
        overloadCost:1, overloadDamage:1
    );
    public static Move QuickAttack = new Move(
        name:"Quick Attack", desc:"1 DMG, +1 ACT\nOverload: +1 DMG", type:MoveType.NORMAL,
        damage:1, cost:2, actionCost:0,
        overloadCost:2, overloadDamage:1
    );
    public static Move Roar = new Move(
        name:"Roar", desc:"+2 ATK\nOverload: +1 ATK", type:MoveType.NORMAL,
        cost: 3,
        overloadCost: 5, overloadDamage:0, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.attackStack += 1 + overloadValue;
        }
    );
    public static Move Ember = new Move(
        name:"Ember", desc:"2 DMG \nOverload: +2 DMG", type:MoveType.FIRE,
        damage:2, cost:2,
        overloadCost:2, overloadDamage:2,
        evolveThreshold:0, evolvedMoveName:"Flamethrower"
    );
    public static Move Flamethrower = new Move(
        name:"Flamethrower", desc:"4 DMG \nOverload: -1 Enemy ATK", type:MoveType.FIRE,
        damage:4, cost:3, overloadCost:8, overloadDamage:0,
        extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.attackStack -= overloadValue;
        }
    );
    public static Move DragonDance = new Move(
        name:"Dragon Dance", desc: "+1 ATK\nOverload: +1 ACT", type:MoveType.DRAGON,
        cost:4, overloadCost:6, overloadDamage:0, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.attackStack += 1;
            source.remainingActions += overloadValue;
        }
    );
    public static Move ScaryFace = new Move(
        name:"Scary Face", desc: "Enemy -1 ATK\nOverload: Enemy -1 ATK", type:MoveType.DARK,
        cost:4, overloadCost:8, overloadDamage:0, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.attackStack -= 1 + overloadValue;
        }
    );
    public static Move ThunderWave = new Move(
        name:"Thunder Wave", desc: "Enemy +1 PAR\nOverload: +1 PAR", type:MoveType.ELECTRIC,
        cost: 3,
        overloadCost: 5, overloadDamage:0, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.paralysisStack = 1 + overloadValue;
        }
    );
    public static Move Spark = new Move(
        name: "Spark", desc:"2 DMG \nOverload: +2 DMG", type:MoveType.ELECTRIC,
        damage:2, cost:2,
        overloadCost:2, overloadDamage:2,
        evolveThreshold:0, evolvedMoveName:"Thunder Fang"
    );
    public static Move ThunderFang = new Move(
        name: "Thunder Fang", desc:"2 DMG \nOverload: Enemy +1 PAR", type:MoveType.ELECTRIC,
        damage:5, cost:5,
        overloadCost:7, overloadDamage:0, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.paralysisStack += overloadValue;
        }
    );
    public static Move IronDefense = new Move(
        name:"Iron Defense", desc: "+1 DEF\nOverload: +1 DEF", type:MoveType.STEEL,
        damage:0, cost:3, overloadCost:3, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.defenseStack += 1 + overloadValue;
        }
    );
    public static Move MagnetRise = new Move(
        name:"Magnet Rise", desc: "+1 DODG\nOverload: +1 DODG", type:MoveType.ELECTRIC,
        cost: 5,
        overloadCost: 10, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.dodgeStack = 1 + overloadValue;
        }
    );
    public static Move Confusion = new Move(
        name:"Confusion", desc: "Enemy CONF\nOverload: +1 CONF", type:MoveType.PSYCHIC,
        cost: 5,
        overloadCost: 15, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            for (var i = 0; i < overloadValue + 1; i++) {
                target.confuse();
            }
        }
    );
    public static Move PoisonSting = new Move(
        name:"Poison Sting", desc: "1 DMG & 1 POI\nOverload: +1 POI", type:MoveType.POISON,
        damage:1, cost:2, overloadCost:1, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.poisonStack += 1 + overloadValue;
        }
    );
    public static Move PoisonJab = new Move(
        name:"Poison Jab", desc: "DMG = POI\nOverload: +1 POI", type:MoveType.POISON,
        damage:0, cost:3, overloadCost:2, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.poisonStack += overloadValue;
            target.currentHealth -= target.poisonStack;
        }
    );
    public static Move Taunt = new Move(
        name:"Taunt", desc: "+1 ATK, Enemy +1 CONF\nOverload: +1 ACT", type:MoveType.DARK,
        damage:0, cost:6, overloadCost:10, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            target.confuse();
            source.attackStack++;
            source.remainingActions += overloadValue;
        }
    );
    public static Move Aquaring = new Move(
        name:"Aqua Ring", desc: "+HP for Duration\nOverload: +1 HP over duration", type:MoveType.WATER,
        damage:0, cost:3, overloadCost:8, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.healthStack += 1 + overloadValue;
            source.healthRegenDuration = source.maxHealthRegenDuration;
        }
    );
    public static Move RainDance = new Move(
        name:"Rain Dance", desc: "+1 DEF\nOverload: +HP", type:MoveType.WATER,
        damage:0, cost:5, overloadCost:6, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.defenseStack++;
            var targetHealth = source.currentHealth + (int) Math.Ceiling(
                (source.maxHealth * 0.2)
            ) * overloadValue;
            source.currentHealth = Math.Min(targetHealth, source.maxHealth);
        }
    );
    public static Move Endure = new Move(
        name:"Endure", desc: "+2 ACT next turn\nOverload: +1 DEF", type:MoveType.NORMAL,
        damage:0, cost:3, overloadCost:7, extraEffects:
        delegate(MonEntity source, MonEntity target, int overloadValue) {
            source.defenseStack += overloadValue;
            source.actionStack += 2;
        }
    );

    public Move getMoveByName(string moveName) {
        // get all static fields of current class and return a constructed move that matches
        return this.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(Move))
            .Select(field => (Move) field.GetValue(null))
            .Where(move => move.name == moveName).First();
    }

    public Move getMoveParent(string moveName) {
        var moveParent =  this.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(Move))
            .Select(field => (Move) field.GetValue(null))
            .Where(move => move.evolvedMoveName == moveName).ToList();

        if (moveParent.Count == 1) {
            return moveParent.First();
        } else {
            return null;
        }
    }

    public string name;
    public string desc;
    public MoveType type;
    public int damage;
    public int cost;
    public int actionCost;
    public int overloadCost;
    public int overloadDamage;
    public int evolveThreshold;
    public string evolvedMoveName;
    public Action<MonEntity, MonEntity, int> extraEffects;


    public Move(
        string name="",
        string desc="",
        MoveType type=MoveType.NORMAL,
        int damage=0,
        int cost=0,
        int actionCost=1,
        int overloadCost=0,
        int overloadDamage=0,
        int evolveThreshold=0,
        string evolvedMoveName="",
        Action<MonEntity, MonEntity, int> extraEffects=null
    ) {
        this.name = name;
        this.desc = desc;
        this.type = type;
        this.damage = damage;
        this.cost = cost;
        this.actionCost = actionCost;
        this.overloadCost = overloadCost;
        this.overloadDamage = overloadDamage;
        this.evolveThreshold = evolveThreshold;
        this.evolvedMoveName = evolvedMoveName;
        this.extraEffects = extraEffects;
    }
}

public enum MoveType {
    NORMAL,
    FIGHTING,
    FLYING,
    POISON,
    GROUND,
    ROCK,
    BUG,
    GHOST,
    STEEL,
    FIRE,
    WATER,
    GRASS,
    ELECTRIC,
    PSYCHIC,
    ICE,
    DRAGON,
    DARK,
    FAIRY
}