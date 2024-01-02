#include "tree.h"
//returns Node t where the string equals val. If t has a prent, the pointer parent will contain its address. 
Node* Tree::search(Node* p, string val, Node*& parent)
{
	if (p == nullptr) return nullptr;//no tree, no val
	if (p->content == val) return p;//found, return Node*
	if (p->isLeaf) return nullptr;//leaf, didn't find val
	//for (auto it = p->responses.begin();it != p->responses.end();it++)
		//A loop that goes through all the pointers of the trees in the central array
	for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
	{
		Node* ezer = search(*it, val, p);
		if (ezer != nullptr)//found
			return ezer;
	}
	return nullptr;//didn't find
}
//Searches and prints starting from a certain root
void Tree::searchAndPrintPath(Node* p, string val)
{

	if (!search(p, val, p)) //didn't find
		//return true;
		return;
	if (p->content == val)
	{
		cout << p->content << "=>";
		//	return true;
	}
	else
	{
		//A loop that goes through all the pointers of the trees in the central array
		for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
		{
			if (search(*it, val, p))
				searchAndPrintPath(*it, val);
		}
		if (p->content == this->root->content)
			cout << p->content << endl;
		else
			cout<<p->content << "=>";
	}
	//return true;
}
void Tree::print(Node* p, int level)
{
	//If the root is empty
	if (p == nullptr)
		return;
	//Prints the requested amount of profits
	for (int i = 0; i < level; i++)
		cout << " ";
	cout << p->content << endl;
	//A loop that goes through all the pointers of the trees in the central array
	for (list<Node*>::iterator it = p->responses.begin(); it != p->responses.end(); it++)
	{
		print(*it, level + 3);
	}
	return;
}
void Tree::deleteAllSubTree(Node* t)
{
	//If the root is empty
	if (t == nullptr) return;
	if (t->isLeaf)
	{
		delete t;
		return;
	}
	//A loop that goes through all the pointers of the trees in the central array
	for (auto it = t->responses.begin(); it != t->responses.end(); it++)
		deleteAllSubTree(*it);
	delete t;
}
//Building a root for a tree with a certain value
void Tree::addRoot(string newval)
{
	if (root)
		deleteAllSubTree(root);
	root = new Node(newval);
}
//Adding a comment to one of the comments
bool Tree::addSon(string fatherdiscussion, string newresponse)
{
	Node* help = search(root, fatherdiscussion, root);
	if (!help) //didn't find
		return false;
	help->isLeaf = false;
	help->responses.push_back(new Node(newresponse));
	return true;
}
//Prints the tree starting from a certain root
bool Tree::printSubTree(Node* curr, string val)
{
	Node* help = search(curr, val, curr);
	if (!help) //didn't find
		return false;
	Tree temp;
	temp.root = help;
	temp.printAllTree();
	temp.root = nullptr;
	return true;
}
//Deleting the entire subtree of the string, including the string itself
bool Tree::deleteSubTree(string val)
{
	Node* help = search(root, val, root);
	if (!help) //didn't find
		return false;
	deleteAllSubTree(help);
	return true;
}