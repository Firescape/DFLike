using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : object
{
    public List<Actor> actorList = new List<Actor>();
    public List<Item2> itemList = new List<Item2>();
    public byte light;
    public Point loc;
    private Block terrain;

    public List<Zone> zoneList = new List<Zone>();
    //public static EntityManager entityManager;


    public Cell(Block block)
    {
        terrain = block;
        light = 0;
    }

    public Cell(byte[] bytes)
    {
        terrain = new Block(bytes);
        light = 0;
    }

    public void moveItemsFromCell(Cell fromCell)
    {
        //you can't move an item to a block cell before removing it from the old block cell
        var tempArray = new List<Item2>();

        tempArray.AddRange(fromCell.itemList);

        fromCell.removeAllItems();

        foreach (var item in tempArray)
            addItem(item);
    }

    public void removeAllItems()
    {
        while (itemList.Count > 0)
            removeItem(itemList[itemList.Count - 1]);
    }

    public void addActor(Actor actor)
    {
        actorList.Add(actor);
    }


    public void moveActorFromCell(Actor actor, Cell fromCell)
    {
        //Debug.Log("moved actor from " + fromCell.loc + " to " + loc);
        addActor(actor);
        fromCell.removeActor(actor);
    }

    public void removeActor(Actor actor)
    {
        actorList.Remove(actor);
    }

    public void addZone(Zone zone)
    {
        zoneList.Add(zone);
    }

    public void removeItem(Item2 item)
    {
        //itemList.Remove(item);
        //Debug.Log(itemList.Contains(item));

        if (itemList.Contains(item))
            itemList.Remove(item);
        else
            Debug.LogError("Attempted to remove an item that wasn't there at " + loc);
    }

    public void addItem(Item2 item)
    {
        item.loc = loc;
        itemList.Add(item);
    }

    public void setTerrain(Block block)
    {
        terrain = block;
    }

    public Block getTerrain()
    {
        return terrain;
    }

    public Mesh2 getMesh(bool[] nearbyList)
    {
        if (terrain.type > 0)
            return terrain.getMesh(nearbyList);
        else if (itemList.Count > 0)
            return itemList[itemList.Count - 1].getMesh();
        else
            return null;
    }

    public List<byte> getBytes()
    {
        //List. bytes<byte> = new List.<byte>();
        //
        //bytes.AddRange(terrain.getBytes());
        //bytes.InsertRange(0, BitConverter.GetBytes(bytes.Count));
        return terrain.getBytes();
    }
}