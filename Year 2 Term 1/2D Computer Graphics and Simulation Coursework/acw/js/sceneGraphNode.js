class SceneGraphNode {
    getType() {
        return this.mType;
    }
    setType(pType) {
        this.mType = pType;
    }
    accept(pVisitor) {
        pVisitor.visit(this);
    }
}