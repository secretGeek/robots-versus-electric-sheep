using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;

namespace nimble_life
{
    public class Board
    {
        public Location Size { get; set; }
        public List<IPiece> Pieces { get; set; }
        public Tile[,] Tiles { get; set; }
        //public int MaxAge { get; set; }
        public int Generation { get; set; }

        internal List<Tile> GetNeighbors(int x, int y, int radius)
        {
            var result = new List<Tile>();

            // We call it a radius, even though it's a square neighborhood.
            // If I can 'see' a distance of '2' squares around me...
            //
            //    2  2  2  2  2
            //    2  1  1  1  2
            //    2  1  O  1  2
            //    2  1  1  1  2
            //    2  2  2  2  2
            //
            // ...then that neighborhood is 5x5.
            // So the width of the neighborhood is (radius * 2 + 1), and the 
            // radius of 2, means an offset from the corners to the centre of [+/-2, +/-2]

            var neighborhoodWidth = (radius * 2 + 1); // Will be an odd number.

            for (int i = 0; i < neighborhoodWidth; i++)
            {
                for (int j = 0; j < neighborhoodWidth; j++)
                {
                    var offsetX = i - radius;
                    var offsetY = j - radius;
                    var newX = x + offsetX;
                    var newY = y + offsetY;
                    if (newX < 0) newX = Size.X - radius;
                    if (newY < 0) newY = Size.Y - radius;
                    if (newX >= Size.X) newX = 0;
                    if (newY >= Size.Y) newY = 0;

                    if (offsetX != 0 || offsetY != 0) // ignore current tile...
                    {
                        result.Add(Tiles[newX, newY]);
                    }
                }
            }

            Rando.Shuffle(result);

            return result;
        }
    }

    public class Tile
    {
        public Grass Grass { get; set; }
        public IAnimal Animal { get; set; }
        public Location Location { get; set; }
    }

    
    public class Grass : IPiece
    {
        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public float MaxEnergy { get; set; } = 100f;
        public float ReGrowthPerTurn { get; set; } = 0.15f;
        public IPiece TakeTurn(Board board)
        {
            // Grass quickly grows... 
            if (Energy < MaxEnergy) Energy = Energy + ReGrowthPerTurn;
            if (Energy > MaxEnergy) Energy = MaxEnergy;

            var green = Math.Min((int)(Energy * (255.0 / MaxEnergy)), 255);

            this.Color = Color.FromArgb(0, green, 0);
            return null;
        }
    }

    public class Tree : IPiece
    {
        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public float MaxEnergy { get; set; } = 200f;
        public float ReGrowthPerTurn { get; set; } = 0.05f;
        public IPiece TakeTurn(Board board)
        {
            // Tree grows slowly...
            if (Energy < MaxEnergy) Energy = Energy + ReGrowthPerTurn;
            if (Energy > MaxEnergy) Energy = MaxEnergy;

            var green = Math.Min((int)(Energy * (128.0 / MaxEnergy)), 128);

            this.Color = Color.FromArgb(0, green, 0);
            return null;
        }
    }

    enum hg //herbivore genes
    {
        WorthMovingTo,
        MatingProbability,
        MinFoodAvailableForBaby,
        EnergyToBaby,
        EnergyRequiredBeforeConsideringOffspring,
        MaxAge,
        MaxMutationFactor,
        SpeciationDistance
    }

    public static class GenesHelper
    {
        public static Dictionary<string, float> GreatestGenes()
        {
            var greatestGenes = new Dictionary<string, float>();
            greatestGenes[hg.EnergyToBaby.ToString()] = (float)0.16;
            greatestGenes[hg.MatingProbability.ToString()] = (float)0.7;
            greatestGenes[hg.MinFoodAvailableForBaby.ToString()] = (float)3.8;
            greatestGenes[hg.WorthMovingTo.ToString()] = (float)3.2;
            greatestGenes[hg.EnergyRequiredBeforeConsideringOffspring.ToString()] = 61;
            greatestGenes[hg.MaxAge.ToString()] = 343;
            greatestGenes[hg.MaxMutationFactor.ToString()] = (float)0.13;
            greatestGenes[hg.SpeciationDistance.ToString()] = (float)25;
            return greatestGenes;
        }
    }

    public class Robot : IPiece, IAnimal
    {
        public float MaxEnergy { get; set; } = 100f;
        public float EnergyRequiredToMove { get; set; } = 5f;
        public float EnergyRequiredToStandStill { get; set; } = 1f;
        public float AmountOfEnergyTakenFromGrass { get; set; } = 15f;
        public float AmountOfEnergyGainedFromGrass { get; set; } = 10f;
        
        private int NeighborhoodRadius { get; set; } = 1;

        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }
        public Dictionary<string, float> Genes { get; set; }
        public void ChooseAction(List<IPiece> neighbors)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            if (this.IsDead) return null;

            IPiece result = null;

            var moveDone = false;
            var currentTile = board.Tiles[this.Location.X, this.Location.Y];

            var neighbors = board.GetNeighbors(this.Location.X, this.Location.Y, this.NeighborhoodRadius);

            var bestNeighbor = currentTile;

            foreach (var t in neighbors)
            {
                if (t.Animal == null && t.Grass.Energy > bestNeighbor.Grass.Energy)
                {
                    bestNeighbor = t;
                }
            }

            if (!moveDone && bestNeighbor != currentTile &&
                (bestNeighbor.Grass.Energy > this.Genes[hg.WorthMovingTo.ToString()]))
            {
                // Move to it!
                currentTile.Animal = null;
                bestNeighbor.Animal = this;
                this.Location.X = bestNeighbor.Location.X;
                this.Location.Y = bestNeighbor.Location.Y;
                this.Energy -= EnergyRequiredToMove; //-1; //TAKES very little energy to move...
                currentTile = board.Tiles[this.Location.X, this.Location.Y];
                moveDone = true;
            }
            else
            {
                // Takes energy to stand still!
                this.Energy -= EnergyRequiredToStandStill;
            }

            if (!moveDone && currentTile.Grass.Energy > AmountOfEnergyTakenFromGrass && this.Energy < MaxEnergy)
            {
                // Chew some.
                currentTile.Grass.Energy -= AmountOfEnergyTakenFromGrass;
                this.Energy += AmountOfEnergyGainedFromGrass; // Note inefficiency
            }

            //neighbors = board.GetNeighbors(this.Location.X, this.Location.Y);

            //var bestLunch = currentTile;
            //foreach (var t in neighbors)
            //{
            //    if (t.Animal != null && (t.Animal is Herbivore) && t.Animal.Energy > bestLunch.Animal.Energy)
            //    {
            //        bestLunch = t;
            //    }
            //}

            //if (bestLunch != currentTile)
            //{
            //    this.Energy = this.Energy + bestLunch.Animal.Energy;

            //    currentTile.Animal = null;
            //    bestNeighbor.Animal = this;
            //    this.Location.X = bestNeighbor.Location.X;
            //    this.Location.Y = bestNeighbor.Location.Y;
            //    //this.Energy -= 1; //TAKES very little energy to move...
            //    currentTile = board.Tiles[this.Location.X, this.Location.Y];
            //    moveDone = true;

            //}

            //if (!moveDone && bestNeighbor != currentTile &&
            //    (bestNeighbor.Grass.Energy > this.Genes[hg.WorthMovingTo.ToString()]))
            //{
            //    // Move to it!
            //    currentTile.Animal = null;
            //    bestNeighbor.Animal = this;
            //    this.Location.X = bestNeighbor.Location.X;
            //    this.Location.Y = bestNeighbor.Location.Y;
            //    this.Energy -= 1; //TAKES very little energy to move...
            //    moveDone = true;
            //}
            //else
            //{
            //    // Takes energy to stand still!
            //    this.Energy -= 1;
            //}

            this.Energy = Math.Min(this.MaxEnergy, this.Energy);

            // Consider trying to mate...
            if (Rando.Next() < this.Genes[hg.MatingProbability.ToString()]
                            && this.Age >= this.AgeOfMaturity)
            {
                var hottestNeighbor = this as IAnimal;
                foreach (var t in neighbors)
                {
                    if (t.Animal != null
                        && t.Animal.Species == this.Species // picky
                        && t.Animal.IsDead == false   // picky!
                        && t.Animal.Age > t.Animal.AgeOfMaturity // picky
                        && t.Animal.Energy >= hottestNeighbor.Energy) //picky
                    {
                        hottestNeighbor = t.Animal;
                    }
                }

                if (hottestNeighbor != this) // picky
                {
                    if (hottestNeighbor.MatingOffers == null) hottestNeighbor.MatingOffers = new List<IAnimal>();
                    // swipe right
                    hottestNeighbor.MatingOffers.Add(this);
                }
            }

            if (this.MatingOffers != null && this.Energy > this.Genes[hg.EnergyRequiredBeforeConsideringOffspring.ToString()])
            {
                var bestOffer = this.MatingOffers
                    .Where(p =>
                            p.IsDead == false
                        && p.Species == this.Species
                        && p.Age > p.AgeOfMaturity)
                        .OrderByDescending(p => p.Energy).FirstOrDefault();

                if (bestOffer != null)
                {
                    // Find an empty neighboring square to plonk a baby on...
                    bestNeighbor = null;

                    foreach (var t in neighbors)
                    {
                        if (t.Animal == null
                            && (bestNeighbor == null || t.Grass.Energy > bestNeighbor.Grass.Energy)
                            && t.Grass.Energy > this.Genes[hg.MinFoodAvailableForBaby.ToString()]) // what kind of a world are we bringing this child into...
                        {
                            bestNeighbor = t;
                        }
                    }

                    if (bestNeighbor != null)
                    {
                        var baby = new Robot
                        {
                            Species = this.Species,
                            Energy = (int)(this.Energy * this.Genes[hg.EnergyToBaby.ToString()]),
                            Age = 0,
                            AgeOfMaturity = this.AgeOfMaturity,
                            Genes = new Dictionary<string, float>()
                        };


                        var MaxMutationFactor = this.Genes[hg.MaxMutationFactor.ToString()];

                        foreach (var d in this.Genes.Keys)
                        {
                            var mutationFactor = (float)1 + (float)((Rando.Next() * MaxMutationFactor) - (MaxMutationFactor / 2));

                            // Cross over and mutation
                            baby.Genes[d] = Rando.Either(bestOffer.Genes[d], this.Genes[d]) * mutationFactor;
                        }

                        // Having babies really takes something out of you.
                        this.Energy -= baby.Energy;

                        baby.Location = new Location()
                        {
                            X = bestNeighbor.Location.X,
                            Y = bestNeighbor.Location.Y
                        };
                        bestNeighbor.Animal = baby;
                        result = baby;

                        // Get rid of all mating offers.
                        this.MatingOffers = null;
                    }
                    else
                    {
                        Debug.WriteLine("No empty tile found.");
                    }
                }
            }

            // shade of gray depends on energy levels
            var gray = Math.Max(0, Math.Min((int)(Energy * (255.0 / this.MaxEnergy)), 255));

            this.Color = Color.FromArgb(0, 0, gray);
            return result;
        }
    }

    public class Herbivore : IPiece, IAnimal
    {
        public float EnergyRequiredToMove { get; set; } = 5f;
        public float EnergyRequiredToStandStill { get; set; } = 1f;
        public float AmountOfEnergyTakenFromGrass { get; set; } = 15f;
        public float AmountOfEnergyGainedFromGrass { get; set; } = 10f;
        public float MaxEnergy { get; set; } = 100f;
        private int NeighborhoodRadius { get; set; } = 1;

        public string[] Actions { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }
        public Dictionary<string, float> Genes { get; set; }

        public void ChooseAction(List<IPiece> neighbors)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            if (this.IsDead) return null;

            IPiece result = null;

            var moveDone = false;
            var currentTile = board.Tiles[this.Location.X, this.Location.Y];
            var neighbors = board.GetNeighbors(this.Location.X, this.Location.Y, this.NeighborhoodRadius);
            var bestNeighbor = currentTile;

            foreach (var t in neighbors)
            {
                if (t.Animal == null && t.Grass.Energy > bestNeighbor.Grass.Energy)
                {
                    bestNeighbor = t;
                }
            }

            if (!moveDone && bestNeighbor != currentTile &&
                (bestNeighbor.Grass.Energy > this.Genes[hg.WorthMovingTo.ToString()]))
            {
                // Move to it!
                currentTile.Animal = null;
                bestNeighbor.Animal = this;
                this.Location.X = bestNeighbor.Location.X;
                this.Location.Y = bestNeighbor.Location.Y;
                this.Energy -= EnergyRequiredToMove; //TAKES energy to move...
                currentTile = board.Tiles[this.Location.X, this.Location.Y];
                moveDone = true;
            }
            else
            {
                // Takes energy to stand still!
                this.Energy -= EnergyRequiredToStandStill;
            }

            if (!moveDone && currentTile.Grass.Energy > AmountOfEnergyTakenFromGrass && this.Energy < MaxEnergy)
            {

                // Chew some.
                currentTile.Grass.Energy -= AmountOfEnergyTakenFromGrass;
                this.Energy += AmountOfEnergyGainedFromGrass; // Note inefficiency
            }

            this.Energy = Math.Min(MaxEnergy, this.Energy);

            // Consider trying to mate...
            if (Rando.Next() < this.Genes[hg.MatingProbability.ToString()]
                            && this.Age >= this.AgeOfMaturity)
            {
                var hottestNeighbor = this as IAnimal;
                foreach (var t in neighbors)
                {
                    if (t.Animal != null
                        && t.Animal.Species == this.Species // picky
                        && t.Animal.IsDead == false   // picky!
                        && t.Animal.Age > t.Animal.AgeOfMaturity // picky
                        && t.Animal.Energy >= hottestNeighbor.Energy) //picky
                    {
                        hottestNeighbor = t.Animal;
                    }
                }

                if (hottestNeighbor != this) // picky
                {
                    if (hottestNeighbor.MatingOffers == null) hottestNeighbor.MatingOffers = new List<IAnimal>();
                    // swipe right
                    hottestNeighbor.MatingOffers.Add(this);
                }
            }

            if (this.MatingOffers != null && this.Energy > this.Genes[hg.EnergyRequiredBeforeConsideringOffspring.ToString()])
            {
                var bestOffer = this.MatingOffers
                    .Where(p =>
                            p.IsDead == false
                        && p.Species == this.Species
                        && p.Age > p.AgeOfMaturity)
                        .OrderByDescending(p => p.Energy).FirstOrDefault();

                if (bestOffer != null)
                {
                    // Find the best empty neighboring square to plonk a baby on...
                    bestNeighbor = null;

                    foreach (var t in neighbors)
                    {
                        if (t.Animal == null
                            && (bestNeighbor == null || t.Grass.Energy > bestNeighbor.Grass.Energy)
                            && t.Grass.Energy > this.Genes[hg.MinFoodAvailableForBaby.ToString()]) // what kind of a world are we bringing this child into...
                        {
                            bestNeighbor = t;
                        }
                    }

                    if (bestNeighbor == null)
                    {
                        Debug.WriteLine("No empty tile found on which to raise a child.");
                    }
                    else
                    {
                        // Okay - we know where the baby will go.
                        // Let's make this baby!

                        var baby = new Herbivore
                        {
                            Species = this.Species,
                            Energy = (int)(this.Energy * this.Genes[hg.EnergyToBaby.ToString()]),
                            Age = 0,
                            AgeOfMaturity = this.AgeOfMaturity,
                            Genes = new Dictionary<string, float>()
                        };


                        var MaxMutationFactor = this.Genes[hg.MaxMutationFactor.ToString()];

                        foreach (var d in this.Genes.Keys)
                        {
                            var mutationFactor = (float)1 + (float)((Rando.Next() * MaxMutationFactor) - (MaxMutationFactor / 2));

                            // Cross over and mutation
                            baby.Genes[d] = Rando.Either(bestOffer.Genes[d], this.Genes[d]) * mutationFactor;
                        }

                        // Having babies really takes something out of you.
                        this.Energy -= baby.Energy;

                        baby.Location = new Location()
                        {
                            X = bestNeighbor.Location.X,
                            Y = bestNeighbor.Location.Y
                        };

                        bestNeighbor.Animal = baby;
                        result = baby;

                        // Get rid of all mating offers.
                        this.MatingOffers = null;
                    }
                }
            }

            // shade of gray depends on energy levels
            var gray = Math.Max(0, Math.Min((int)(Energy * (255.0 / this.MaxEnergy)), 255));

            this.Color = Color.FromArgb(gray, gray, gray);
            return result;
        }
    }

    public class Omnivore : IPiece, IAnimal
    {
        public string[] Actions { get; set; }
        public Dictionary<string, float> Genes { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }
        public float MaxEnergy { get; set; } = 100f;
        public float EnergyRequiredToMove { get; set; } = 5f;
        public float EnergyRequiredToStandStill { get; set; } = 1f;
        public float AmountOfEnergyTakenFromGrass { get; set; } = 15f;
        public float AmountOfEnergyGainedFromGrass { get; set; } = 10f;

        public void ChooseAction(List<IPiece> neighbors)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            return null;
        }
    }

    public class Carnivore : IPiece, IAnimal
    {
        public string[] Actions { get; set; }
        public Dictionary<string, float> Genes { get; set; }
        public Color Color { get; set; }
        public float Energy { get; set; }
        public Location Location { get; set; }
        public List<IAnimal> MatingOffers { get; set; }
        public string Species { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int AgeOfMaturity { get; set; }

        public float MaxEnergy { get; set; } = 100f;

        public float EnergyRequiredToMove { get; set; } = 5f;
        public float EnergyRequiredToStandStill { get; set; } = 1f;
        public float AmountOfEnergyTakenFromGrass { get; set; } = 15f;
        public float AmountOfEnergyGainedFromGrass { get; set; } = 10f;

        public void ChooseAction(List<IPiece> neighbors)
        {
            throw new NotImplementedException();
        }

        public void Eat(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void OfferToMate(IAnimal animal)
        {
            throw new NotImplementedException();
        }

        public IPiece TakeTurn(Board board)
        {
            return null;
        }
    }

    public interface IAnimal : IPiece
    {
        void ChooseAction(List<IPiece> neighbors);
        Dictionary<string, float> Genes { get; set; }
        void Eat(IPiece piece);
        void Move();
        bool IsDead { get; set; }
        int Age { get; set; }
        int AgeOfMaturity { get; set; }
        void OfferToMate(IAnimal animal);
        List<IAnimal> MatingOffers { get; set; }
        string Species { get; set; }

        float EnergyRequiredToMove { get; set; } //= 5f;
        float EnergyRequiredToStandStill { get; set; } //= 1f;
        float AmountOfEnergyTakenFromGrass { get; set; } //= 15f;
        float AmountOfEnergyGainedFromGrass { get; set; } //= 10f;
    }

    public interface IPiece
    {
        Location Location { get; set; }
        float Energy { get; set; }
        IPiece TakeTurn(Board board);
        string[] Actions { get; set; }
        Color Color { get; set; }

        float MaxEnergy { get; set; } //= 100f
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
