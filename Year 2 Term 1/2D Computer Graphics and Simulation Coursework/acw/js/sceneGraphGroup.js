class Group extends SceneGraphNode {
    constructor() {
        super();
        this.setType("Group");
        this.mChildren = [];
        
    }
    getNumberOfChildren() {
        return this.mChildren.length;
    }
    getChildAt(pIndex) {
        return this.mChildren[pIndex];
    }
    addChild(pChild) {
        this.mChildren.push(pChild);
    }
}