class Geometry extends SceneGraphNode {
    constructor(pContext, pDrawableObject) {
        super();
        this.setType("Geometry");
        this.setContext(pContext);
        this.setDrawableObject(pDrawableObject);
    }
    //#region Getters & Setters
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
    //#endregion
    
    draw(pWorldTransformMatrix) {

        pWorldTransformMatrix.setTransform(this.getContext());

        this.getDrawableObject().draw(this.getContext());
    }
}