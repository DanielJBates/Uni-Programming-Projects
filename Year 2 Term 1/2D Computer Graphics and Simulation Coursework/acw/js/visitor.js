class RenderVistor {
    constructor() {
        this.mTransformStack = [];
    }
    
    visit(pNode) {
        if(pNode.getType() === 'Group') {
            this.visitGroup(pNode);
        }
        else if(pNode.getType() === 'Transform') {
            this.visitTransform(pNode);
        }
        else if(pNode.getType() === 'Geometry') {
            this.visitGeometry(pNode);
        }
    }
    visitGroup(pNode) {
        for(var i = 0; i < pNode.getNumberOfChildren(); i += 1) {
            var child = pNode.getChildAt(i);
            child.accept(this);
        }
    }
    visitTransform(pNode) {
        this.pushTransform(pNode.getTransform());
        this.visitGroup(pNode);
        this.popTransform();
    }
    visitGeometry(pNode) {
        pNode.draw(this.peekTransform());
    }
    popTransform() {
        this.mTransformStack.pop();
    }
    peekTransform() {
        return this.mTransformStack[this.mTransformStack.length - 1];
    }
    pushTransform(pNode) {
        if(this.mTransformStack.length == 0) {
            this.mTransformStack.push(pNode);
        }
        else {
            var currentTransform, productTransform;

            currentTransform = this.peekTransform();

            productTransform = currentTransform.multiply(pNode);

            this.mTransformStack.push(productTransform);
        }
    }
}