public enum PuzzleState
{
	Unsolved,
	Solved,
	PlayerHitCow,
	PlayerHitSheep,
	PlayerHitTractor,
	CowHitTractor,
	CowHitSheep,
	OutOfMoves,
	MissedSomeGrass,
	UnknownCollision
};

public enum ActorType
{
	Generic,
	Farmer,
	Cow,
	Tractor,
	Sheep
};

public enum TileType
{
	Blank,
	Grass,
	Tree,
	BarnStart,
	BarnEnd,
	TrackS,
	TrackR
};