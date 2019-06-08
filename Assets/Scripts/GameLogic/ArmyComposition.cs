﻿using System;

namespace Assets.Scripts
{
	/// <summary>
    /// Implementation of army composition a game
    /// </summary>
    public class ArmyComposition
    {
        /// <summary>
        /// Number of spearmen in army
        /// </summary>
        public int Spearmen { get; private set; }
        
        /// <summary>
        /// Number of archers in army
        /// </summary>
        public int Archers { get; private set; }
        
        /// <summary>
        /// Number of cavalrymen in army
        /// </summary>
        public int Cavalrymen { get; private set; }
        
        /// <summary>
        /// Army experience
        /// </summary>
        public double Experience { get; private set; }

        public ArmyComposition(int spearmen, int archers, int cavalrymen, double experience = 1)
        {
            Spearmen = spearmen;
            Archers = archers;
            Cavalrymen = cavalrymen;
            Experience = experience;
        }

        /// <summary>
        /// Converts army composition to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Spearmen: " + Spearmen + "\n" + "Archers:    " + Archers + "\n" + "Cavalry:    " + Cavalrymen + "\n" +
                   "Experience: " + Math.Round(Experience, 2);
        }

        /// <summary>
        /// Merges two armycompositions
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ArmyComposition Merge(ArmyComposition first, ArmyComposition second)
        {
            var newExperience = (first.TotalUnitQuantity() * first.Experience +
                                   second.TotalUnitQuantity() * second.Experience) /
                                   (first.TotalUnitQuantity() + second.TotalUnitQuantity());

            return new ArmyComposition(first.Spearmen + second.Spearmen,
                                       first.Archers + second.Archers,
                                       first.Cavalrymen + second.Cavalrymen,
                                       newExperience);
        }

        /// <summary>
        /// Perform fight between two army compositions
        /// </summary>
        /// <param name="winnerArmyComposition"></param>
        /// <param name="loserArmyComposition"></param>
        /// <returns></returns>
        public static ArmyComposition Fight(ArmyComposition winnerArmyComposition, 
                                            ArmyComposition loserArmyComposition)
        {
            var powerDifference = winnerArmyComposition.ArmyPower() - loserArmyComposition.ArmyPower();
            var mortalityRate = Math.Sqrt(powerDifference / winnerArmyComposition.ArmyPower());
            var experienceIncrease = 1 + Math.Pow(loserArmyComposition.ArmyPower() / winnerArmyComposition.ArmyPower(), 2);
            return winnerArmyComposition.ArmyCompositionAfterFight(mortalityRate, 
                            winnerArmyComposition.Experience * experienceIncrease);
        }

        private ArmyComposition ArmyCompositionAfterFight(double mortalityRate, double experience)
        {
            return new ArmyComposition((int)Math.Ceiling(Spearmen * mortalityRate),
                (int)Math.Ceiling(Archers * mortalityRate), (int)Math.Ceiling(Cavalrymen * mortalityRate), experience);
        }

        /// <summary>
        /// Checks that first army is more powerful than second
        /// </summary>
        /// <param name="firstArmyComposition"></param>
        /// <param name="secondArmyComposition"></param>
        /// <returns></returns>
        public static bool IsFirstWinner(ArmyComposition firstArmyComposition,
                                          ArmyComposition secondArmyComposition)
        {
            var firstArmyPower = firstArmyComposition.ArmyPower();
            var secondArmyPower = secondArmyComposition.ArmyPower();
            return firstArmyPower >= secondArmyPower;
        }

        /// <summary>
        /// Calculates total number of units in army
        /// </summary>
        /// <returns></returns>
        public int TotalUnitQuantity()
        {
            return Spearmen + Archers + Cavalrymen;
        }

        /// <summary>
        /// Calculates army power
        /// </summary>
        /// <returns></returns>
        public double ArmyPower()
        {
            return TotalUnitQuantity() * Experience;
        }

        /// <summary>
        /// Removes army part
        /// </summary>
        /// <param name="spearmen"></param>
        /// <param name="archers"></param>
        /// <param name="cavalrymen"></param>
        public void DeleteArmyPart(int spearmen, int archers, int cavalrymen)
        {
            Spearmen = Math.Max(0, Spearmen - spearmen);
            Archers = Math.Max(0, Archers - archers);
            Cavalrymen = Math.Max(0, Cavalrymen - cavalrymen);
        }
    }
}