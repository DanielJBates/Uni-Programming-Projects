class Polygon {
    constructor(pVectorArray, pFillColour, pStrokeColour) {
        this.setVectorArray(pVectorArray);
        this.setFillColour(pFillColour);
        this.setStrokeColour(pStrokeColour);
    }
    getVectorArray() {
        return this.mVectorArray;
    }
    setVectorArray(pVectorArray) {
        this.mVectorArray = pVectorArray;
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
    draw(pContext) {
        var x, y, vectorArray;

        vectorArray = this.getVectorArray();

        pContext.fillStyle = this.getFillColour();
        pContext.strokeStyle = this.getStrokeColour();

        pContext.beginPath();
        for(var i = 0; i < vectorArray.length; i += 1) {
            x = vectorArray[i].getX();
            y = vectorArray[i].getY();

            if(i == 0) {
                pContext.moveTo(x, y);
            }
            else {
                pContext.lineTo(x , y);
            }
        }
        pContext.closePath();
        pContext.fill();
        pContext.stroke();      
    }
}