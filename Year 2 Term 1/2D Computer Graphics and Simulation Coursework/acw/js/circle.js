class Circle {
    constructor(pRadius, pFillColour, pStrokeColour) {
        this.setRadius(pRadius);
        this.setFillColour(pFillColour);
        this.setStrokeColour(pStrokeColour);
    }
    //#region Getters & Setters
    getRadius() {
        return this.mRadius;
    }
    setRadius(pRadius) {
        this.mRadius = pRadius;
    }
    getFillColour() {
        return this.mFillColour;
    }
    setFillColour(pFillColour) {
        this.mFillColour = pFillColour;
    }
    getStrokeColour() {
        return this.mStrokeColour;
    }
    setStrokeColour(pStrokeColour) {
        this.mStrokeColour = pStrokeColour;
    }
    //#endregion
    draw(pContext) {
        pContext.beginPath();
        pContext.fillStyle = this.getFillColour();
        pContext.strokeStyle = this.getStrokeColour();
        pContext.arc(0, 0, this.getRadius(), 0, (Math.PI * 2));
        pContext.closePath();
        pContext.fill();
        pContext.stroke();
    }
}