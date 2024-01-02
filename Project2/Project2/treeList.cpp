#include "tree.h"

//destructor: Deletes all trees in the list. Returns the list to the state of an empty list
treeList::~treeList()
{
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
		(**it).~Tree();
	trees.clear();
}
//Creating a new discussion tree.
void treeList::addNewTree(string s)
{
	Tree* temp = new Tree();
	temp->root = new Node(s);
	trees.push_front(temp);
}
//Deleting a discussion tree (given a pointer to the root of the tree).
void treeList::deleteTree(Tree* t)
{
	trees.remove(t);
}
//Searching for a string in all vertices of all trees
void treeList::searchAndPrint(string val)
{
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).search((**it).root, val, (**it).root))//found
		{
			cout << endl;
			Tree temp;
			temp.root =(**it).search((**it).root, val, (**it).root);
			temp.printAllTree();
			(**it).searchAndPrintPath(val);
			temp.root = nullptr;
		}
	}
}
//Searching for a string in all vertices of all trees,
bool treeList::addResponse(string rt, string prnt, string res)
{
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		//Checking if this is the root we want
		if ((**it).root->content == rt)
		{
			//If the son is not found, you cannot add a comment to him
			if (!(**it).search((**it).root, prnt, (**it).root))
				return false;
			(**it).addSon(prnt, res);
			return true;
		}
	}
	return false;//didn't find
}
//Deleting a discussion in a particular tree
bool treeList::delResponse(string rt, string res)
{	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		//Checking if this is the root we want
		if ((**it).root->content == rt)
		{
			//If the son is not found, you cannot delete a comment for him
			if (!(**it).search((**it).root, res, (**it).root))
				return false;
			(**it).deleteSubTree(res);
			if (rt == res)
				trees.remove(*it);
			return true;
		}
	}
	return false;
}
void treeList::printTree(string rt)
{
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
	{
		if ((**it).root->content == rt)
		{
			(**it).printAllTree();
		}
	}
}
void treeList::printAllTrees()
{
	//A loop that goes through all the pointers of the trees in the central array
	int i = 1;
	for (list<Tree*>::iterator it = trees.begin() ; it != trees.end(); it++, i++)
	{
		cout << "Tree #" << i << endl;
		(**it).printAllTree();
		cout << endl;
	}
}
void treeList::printSubTree(string rt, string s)
{
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Tree*>::iterator it = trees.begin(); it != trees.end(); it++)
		if ((**it).root->content == rt)
		{
			(**it).printSubTree(s);
		}
}