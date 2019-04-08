﻿using System;
using System.Collections.Generic;

namespace TheWorld
{
	/// <summary>
	/// Area represents a location in The World.
	/// Areas can be traveled between and are linked to their neighbors.
	/// Areas can contain objects which can be looked at or even interacted with.
	/// </summary>
	public class Area
	{
		/// <summary>
		/// The neighboring areas, indexed by Keyword for travel.
		/// </summary>
		protected Dictionary<string, Area> NeighboringAreas;

		/// <summary>
		/// The items found in this Area, indexed by Unique Name.
		/// </summary>
		protected Dictionary<string, Item> Items;

		/// <summary>
		/// The creatures found in this Area, indexed by Unique Name.
		/// </summary>
		protected Dictionary<string, Creature> Creatures;

        /// <summary>
        /// Name of this Area.
        /// </summary>
		public string Name
		{
			get;
			set;
		}

        /// <summary>
        /// Description of this Area.
        /// </summary>
		public string Description
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TheWorld.Area"/> class.
		/// </summary>
		public Area()
		{
			NeighboringAreas = new Dictionary<string, Area>();
			Items = new Dictionary<string, Item>();
			Creatures = new Dictionary<string, Creature>();
		}

		/// <summary>
		/// Looks at thing.
		/// </summary>
		/// <returns>The Description of the thing you are looking at.</returns>
		/// <param name="thing">Thing.</param>
		public string LookAt( string thing )
		{
			if(Items.ContainsKey(thing))
			{
				return string.Format("{0} - {1}", Items [thing].Name, Items [thing].Description);
			} else if(Creatures.ContainsKey(thing))
			{
				return string.Format("{0} - {1}", Creatures [thing].Name, Creatures [thing].Description);
			} else if(NeighboringAreas.ContainsKey(thing))
			{
				return NeighboringAreas [thing].Name;
			}
				
			throw new WorldException("I don't see anything like that...");
		}

		/// <summary>
		/// Looks around.
		/// </summary>
		/// <returns>The around.</returns>
		public string LookAround()
		{
			string longDescription = this.ToString() + Environment.NewLine;
			foreach(string key in Items.Keys)
			{
				longDescription += string.Format("There is a [{0}]. {1}", key, Environment.NewLine);
			}
			foreach(string key in Creatures.Keys)
			{
				longDescription += string.Format("You see a [{0}]. {1}", key, Environment.NewLine);
			}

			foreach(string keyword in NeighboringAreas.Keys)
			{
				longDescription += string.Format("If you go [{0}] there is a {1}.{2}",
				                                  keyword,
				                                  NeighboringAreas [keyword].Name,
				                                  Environment.NewLine);
			}
			return longDescription;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() => string.Format("{0}{1}{2}", Name, Environment.NewLine, Description);

		/// <summary>
		/// Adds the neighbor.
		/// </summary>
		/// <param name="neighbor">Neighbor.</param>
		/// <param name="keyword">Keyword. Must be unique in this area.</param>
		/// <exception cref="WorldException">Throws an WorldException if the keyword is already used in this area.</exception>
		public void AddNeighbor( Area neighbor, string keyword )
		{
			if(NeighboringAreas.ContainsKey(keyword))
				throw new WorldException("That keyword is already taken");
			
			NeighboringAreas.Add(keyword, neighbor);
		}

		/// <summary>
		/// Gets the neighbor with the given keyword.
		/// </summary>
		/// <returns>The neighbor.</returns>
		/// <param name="keyword">Keyword.</param>
		/// <exception cref="WorldException">If there is no neighbor with the given keyword.</exception>
		public Area GetNeighbor( string keyword )
		{
			if(!NeighboringAreas.ContainsKey(keyword))
				throw new WorldException("I can't go that way...");

			return NeighboringAreas [keyword];
		}

		/// <summary>
		/// Adds the item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="uid">Unique Name for the item.  Must be unique in this area.</param>
		/// <exception cref="WorldException">Throws an WorldException if the uid is already used in this area.</exception>
		public void AddItem( Item item, string uid )
		{
			if(Items.ContainsKey(uid))
				throw new WorldException("There is already an Item in this area with that Unique Name.");
			
			Items.Add(uid, item);
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="uid">Uid.</param>
		public Item GetItem( string uid )
		{
			if(!Items.ContainsKey(uid))
				throw new WorldException("I don't see anything like that...");

			return Items [uid];
		}

        public bool HasItem(string uid) => Items.ContainsKey(uid);

		/// <summary>
		/// Adds the creature.
		/// </summary>
		/// <param name="creature">Creature.</param>
		/// <param name="uid">Unique name for the creature.  Must be unique in this area.</param>
		/// <exception cref="WorldException">Throws an WorldException if the uid is already used in this area.</exception>
		public void AddCreature( Creature creature, string uid )
		{
			if(Creatures.ContainsKey(uid))
				throw new WorldException("There is already a Creature with that unique name in this area.");

			Creatures.Add(uid, creature);
		}

		/// <summary>
		/// Gets the creature.
		/// </summary>
		/// <returns>The creature.</returns>
		/// <param name="uid">Uid.</param>
		public Creature GetCreature( string uid )
		{
			if(!Creatures.ContainsKey(uid))
				throw new WorldException("I don't see that around here...");

			return Creatures [uid];
		}


        /// <summary>
        /// Remove a creature after it has been killed.  Or for some other reason.
        /// </summary>
        /// <param name="uid"></param>
        public void RemoveCreature(string uid)
        {
            if(!Creatures.ContainsKey(uid))
                throw new WorldException("I don't see that around here...");

            Creatures.Remove(uid);
        }

		/// <summary>
		/// Determines whether this instance can go the specified direction.
		/// </summary>
		/// <returns><c>true</c> if this instance can go the specified direction; otherwise, <c>false</c>.</returns>
		/// <param name="direction">Direction.</param>
		public bool CanGo( string direction ) => NeighboringAreas.ContainsKey(direction);

	}
}
