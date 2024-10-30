using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;

public class Mouse : Area2D
{
	private Random random = new Random();

	public int Age { get; set; }

	public Sex Sex { get; set; } 

	public Genotype Genotype { get; set; }

	public Mouse Partner { get; set; }

	public (float, float, float, float) Borders { get; set; }

	public void Initialize(Sex sex, Genotype genotype)
	{
		Age = 0;
		Sex = sex;
		if (sex == Sex.Male)
		{
			this.ApplyScale(new Vector2(1.20f, 1.20f));
		}

		Genotype = genotype;
		if (this.Genotype == Genotype.aa)
		{
			this.GetNode<Sprite>("Sprite").Material = Utils.ColorInverter;
		}
	}

	private void AgeAndDie()
	{
		Age += 1;
		if (Age > 10 && random.NextDouble() > 0.95)
		{
			// 5% chance to die when age is over 10
			QueueFree();
		}
	}

	public bool IsRandomlySelectedGameteDominant()
	{
		if (this.Genotype == Genotype.AA)
			return true;
		if (this.Genotype == Genotype.aa)
			return false;
		return random.NextDouble() > 0.5; // Heterozygous, so 50/50 chance
	}

	public void SeekAPartner()
	{
		int neighboringConstantSquared = 50 * 50;
		List<Mouse> neighboringPotentialMates = new List<Mouse>(); ; // radius 50
		int nearbyConstantSquared = 100 * 100;
		List<Mouse> nearbyPotentialMates = new List<Mouse>(); ; // radius 100

		var mice = GetTree().GetNodesInGroup("mice");
		for (int i = 0; i < mice.Count; i++)
		{
			Mouse mouse = mice[i] as Mouse;
			if (mouse.Position == this.Position)
				continue;
			if (mouse.Sex == this.Sex)
				continue;
			float distanceSquared = mouse.Position.DistanceSquaredTo(this.Position);

			if (distanceSquared <= neighboringConstantSquared)
				neighboringPotentialMates.Add(mouse);

			if (distanceSquared <= nearbyConstantSquared)
				nearbyPotentialMates.Add(mouse);

			if (nearbyPotentialMates.Count >= 10)
				return; // Too many options I guess?
		}

		if (neighboringPotentialMates.Count == 0)
			return;
		if (nearbyPotentialMates.Count >= 10)
			return; // Too many options I guess?

		if (neighboringPotentialMates[0].Partner == null)
		{
			this.Partner = neighboringPotentialMates[0];
			neighboringPotentialMates[0].Partner = this;
		}
	}

	public Mouse[] Reproduce(PackedScene mouseScene)
	{
		if (this.Partner == null)
			return null;

		if (this.IsQueuedForDeletion())
			return null;

		Mouse[] children = new Mouse[4];
		// Create 4 children with a random sex and a random gamete from each parent.
		for (int i = 0; i < 4; i++)
		{
			Mouse child = mouseScene.Instance() as Mouse;
			if (random.NextDouble() > 0.5)
				child.Sex = Sex.Male;
			else
				child.Sex = Sex.Female;

			bool isParentOneGameteDominant = this.IsRandomlySelectedGameteDominant();
			bool isParentTwoGameteDominant = this.Partner.IsRandomlySelectedGameteDominant();
			if (isParentOneGameteDominant && isParentTwoGameteDominant)
				child.Genotype = Genotype.AA;
			else if (!isParentOneGameteDominant && !isParentTwoGameteDominant)
				child.Genotype = Genotype.aa;
			else
				child.Genotype = Genotype.Aa;

			child.Initialize(child.Sex, child.Genotype);
			child.Position = this.Position;
			Utils.Move(child);
			children[i] = child;
		}

		this.QueueFree();
		this.Partner.QueueFree();
		this.Partner.Partner = null;
		this.Partner = null;
		return children;
	}

	private void DieIfPredated(int predationChance, float camouflage)
	{
		if (random.NextDouble() < predationChance / 100.0 - camouflage)
			this.QueueFree();
	}

	public void Behave(bool predationEnabled, int predationChance, float camouflage)
	{
		Utils.Move(this);
		AgeAndDie();

		if (predationEnabled)
			DieIfPredated(predationChance, camouflage);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
