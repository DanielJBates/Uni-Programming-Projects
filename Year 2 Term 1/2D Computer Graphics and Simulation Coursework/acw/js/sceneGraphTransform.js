class Transform extends Group {
    constructor(pMatrix) {
        super();
        this.setType("Transform");
        this.setTransform(pMatrix);
    }

    getTransform() {
        return this.mLocalMatrix;
    }
    setTransform(pMatrix) {
         this.mLocalMatrix = pMatrix;
    }
}