#pragma once
#include <iostream>
#include <list>
#include <string>
using namespace std;

class Node;

//Node: each Node in the discussion tree
class Node
{
	void removeSonValue(string v);
public:
	list<Node*> responses;
	string content;
	bool isLeaf;
	//Constructor
	Node(string v) { isLeaf = true;  content = v; }

	friend class Tree;
};


//Tree: the discussion Tree
class Tree
{
	Node* root;
	Node* search(Node* p, string val, Node*& parent);
	//returns Node t where the string equals val. If t has a prent, the pointer parent will contain its address. 
	//Searches and prints starting from a certain root
	void searchAndPrintPath(Node* p, string val);
	void print(Node* p, int level = 0);
public:
	//Constructor: Creates an empty tree
	Tree() { root = NULL; }
	//destructor
	~Tree() {
		deleteAllSubTree(root);
		root = 0;
	}
	void deleteAllSubTree(Node* t);
	//Building a root for a tree with a certain value
	void addRoot(string newval);
	//Adding a comment to one of the comments
	bool addSon(string fatherdiscussion, string newresponse);
	bool search(string val)
	{
		Node* parent;
		if (search(root, val, parent))
			cout << root->content << endl;
	}
	bool searchAndPrintPath(string val)
	{
		searchAndPrintPath(root, val);
		//bool flag = searchAndPrintPath(root, val);
		//return flag;
		return true;
	}
	//All tree printing
	void printAllTree() { print(root); }
	//Given a certain value, searching for a vertex with this string, and printing the entire subtree starting from the text of that string up to all the leaves, in a hierarchical form
	void printSubTree(string val) { printSubTree(root, val); }
	//Prints the tree starting from a certain root
	bool printSubTree(Node* curr, string val);
	//Deleting the entire subtree of the string, including the string itself
	bool deleteSubTree(string val);
	friend class treeList;
};


class treeList
{
	list<Tree*> trees;

public:
	//Constructor: Constructs an empty list
	treeList() { list<Tree*> trees; }
	//destructor: Deletes all trees in the list. Returns the list to the state of an empty list
	~treeList();
	//Creating a new discussion tree.
	void addNewTree(string s);
	//Deleting a discussion tree (given a pointer to the root of the tree).
	void deleteTree(Tree*);
	//Searching for a string in all vertices of all trees
	void searchAndPrint(string val);
	//Searching for a string in all vertices of all trees,
	bool addResponse(string rt, string prnt, string res);
	//Deleting a discussion in a particular tree
	bool delResponse(string rt, string res);
	//Functions for partial or complete prints
	void printTree(string rt);
	void printAllTrees();
	void printSubTree(string rt, string s);
};