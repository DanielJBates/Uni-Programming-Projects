class Geometry extends SceneGraphNode {
    constructor(pContext, pDrawableObject) {
        super();
        this.setType("Geometry");
        this.setContext(pContext);
        this.setDrawableObject(pDrawableObject);
    }

    getContext() {
        return this.mContext;
    }
    setContext(pContext) {
        this.mContext = pContext
    }
    getDrawableObject() {
        return this.mDrawableObject;
    }
    setDrawableObject(pDrawableObject) {
        this.mDrawableObject = pDrawableObject;
    }
    draw(pWorldTransformMatrix) {

        pWorldTransformMatrix.setTransform(this.getContext());

        this.getDrawableObject().draw(this.getContext());
    }
}